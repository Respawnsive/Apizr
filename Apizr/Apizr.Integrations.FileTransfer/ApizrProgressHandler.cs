using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System;
using System.ComponentModel;
using System.Net;
using System.Collections.Generic;
using System.Net.Http.Headers;

namespace Apizr.Integrations.FileTransfer
{
    /// <summary>
    /// The <see cref="ApizrProgressHandler" /> provides a mechanism for getting progress event notifications
    /// when sending and receiving data in connection with exchanging HTTP requests and responses.
    /// Register event handlers for the events <see cref="ApizrProgressHandler.HttpSendProgress" /> and <see cref="ApizrProgressHandler.HttpReceiveProgress" />
    /// to see events for data being sent and received.
    /// </summary>
    public class ApizrProgressHandler : DelegatingHandler
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Net.Http.Handlers.ProgressMessageHandler" /> class.
        /// </summary>
        public ApizrProgressHandler()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Net.Http.Handlers.ProgressMessageHandler" /> class.
        /// </summary>
        /// <param name="innerHandler">The inner handler to which this handler submits requests.</param>
        public ApizrProgressHandler(HttpMessageHandler innerHandler)
          : base(innerHandler)
        {
        }

        protected override async Task<HttpResponseMessage> SendAsync(
          HttpRequestMessage request,
          CancellationToken cancellationToken)
        {
            var trackProgress = request.ContainsApizrProgress();
            if (trackProgress)
                AddRequestProgress(request);

            var response = await base.SendAsync(request, cancellationToken);

            if (trackProgress && response is {Content: { }})
            {
                cancellationToken.ThrowIfCancellationRequested();
                await AddResponseProgressAsync(request, response);
            }

            return response;
        }

        /// <summary>
        /// Raises the <see cref="E:System.Net.Http.Handlers.ProgressMessageHandler.HttpSendProgress" /> event.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="e">The <see cref="T:System.Net.Http.Handlers.HttpProgressEventArgs" /> instance containing the event data.</param>
        protected internal virtual void OnHttpRequestProgress(
          HttpRequestMessage request,
          HttpProgressEventArgs e)
        {
            if (request.TryGetApizrProgress(out var apizrProgress))
                apizrProgress.Report(new ApizrProgressEventArgs(ApizrProgressType.Request, e));
        }

        /// <summary>
        /// Raises the <see cref="E:System.Net.Http.Handlers.ProgressMessageHandler.HttpReceiveProgress" /> event.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="e">The <see cref="T:System.Net.Http.Handlers.HttpProgressEventArgs" /> instance containing the event data.</param>
        protected internal virtual void OnHttpResponseProgress(
          HttpRequestMessage request,
          HttpProgressEventArgs e)
        {
            if (request.TryGetApizrProgress(out var apizrProgress))
                apizrProgress.Report(new ApizrProgressEventArgs(ApizrProgressType.Response, e));
        }

        private void AddRequestProgress(HttpRequestMessage request)
        {
            if (request?.Content == null)
                return;

            var httpContent = (HttpContent)new ProgressContent(request.Content, this, request);
            request.Content = httpContent;
        }

        private async Task<HttpResponseMessage> AddResponseProgressAsync(
          HttpRequestMessage request,
          HttpResponseMessage response)
        {
            var handler = this;
            var httpContent = (HttpContent)new StreamContent((Stream)new ProgressStream(await response.Content.ReadAsStreamAsync(), handler, request, response));
            response.Content.Headers.CopyTo(httpContent.Headers);
            response.Content = httpContent;
            return response;
        }
    }
    //{
    //    /// <inheritdoc />
    //    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    //    {
    //        return base.SendAsync(request, cancellationToken);
    //    }

    //    /// <inheritdoc />
    //    protected override void OnHttpRequestProgress(HttpRequestMessage request, HttpProgressEventArgs e)
    //    {
    //        base.OnHttpRequestProgress(request, e);

    //        if(request.TryGetApizrProgress(out var apizrProgress))
    //            apizrProgress.Report(new ApizrProgressEventArgs(ApizrProgressType.Request, e));
    //    }

    //    /// <inheritdoc />
    //    protected override void OnHttpResponseProgress(HttpRequestMessage request, HttpProgressEventArgs e)
    //    {
    //        base.OnHttpResponseProgress(request, e);

    //        if (request.TryGetApizrProgress(out var apizrProgress))
    //            apizrProgress.Report(new ApizrProgressEventArgs(ApizrProgressType.Response, e));
    //    }
    //}

    public class HttpProgressEventArgs : ProgressChangedEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Net.Http.Handlers.HttpProgressEventArgs" /> with the parameters given.
        /// </summary>
        /// <param name="progressPercentage">The percent completed of the overall exchange.</param>
        /// <param name="userToken">Any user state provided as part of reading or writing the data.</param>
        /// <param name="bytesTransferred">The current number of bytes either received or sent.</param>
        /// <param name="totalBytes">The total number of bytes expected to be received or sent.</param>
        public HttpProgressEventArgs(
            int progressPercentage,
            object userToken,
            long bytesTransferred,
            long? totalBytes)
            : base(progressPercentage, userToken)
        {
            BytesTransferred = bytesTransferred;
            TotalBytes = totalBytes;
        }

        /// <summary>Gets the current number of bytes transferred.</summary>
        public long BytesTransferred { get; private set; }

        /// <summary>
        /// Gets the total number of expected bytes to be sent or received. If the number is not known then this is null.
        /// </summary>
        public long? TotalBytes { get; private set; }
    }

    /// <summary>
    /// Wraps an inner <see cref="T:System.Net.Http.HttpContent" /> in order to insert a <see cref="T:System.Net.Http.Handlers.ProgressStream" /> on writing data.
    /// </summary>
    internal class ProgressContent : HttpContent
    {
        private readonly HttpContent _innerContent;
        private readonly ApizrProgressHandler _handler;
        private readonly HttpRequestMessage _request;

        public ProgressContent(
            HttpContent innerContent,
            ApizrProgressHandler handler,
            HttpRequestMessage request)
        {
            _innerContent = innerContent;
            _handler = handler;
            _request = request;
            innerContent.Headers.CopyTo(Headers);
        }

        protected override Task SerializeToStreamAsync(Stream stream, TransportContext context) => _innerContent.CopyToAsync((Stream)new ProgressStream(stream, _handler, _request, (HttpResponseMessage)null));

        protected override bool TryComputeLength(out long length)
        {
            var contentLength = _innerContent.Headers.ContentLength;
            if (contentLength.HasValue)
            {
                length = contentLength.Value;
                return true;
            }
            length = -1L;
            return false;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (!disposing)
                return;
            _innerContent.Dispose();
        }
    }

    /// <summary>
    /// This implementation of <see cref="T:System.Net.Http.Internal.DelegatingStream" /> registers how much data has been
    /// read (received) versus written (sent) for a particular HTTP operation. The implementation
    /// is client side in that the total bytes to send is taken from the request and the total
    /// bytes to read is taken from the response. In a server side scenario, it would be the
    /// other way around (reading the request and writing the response).
    /// </summary>
    internal class ProgressStream : DelegatingStream
    {
        private readonly ApizrProgressHandler _handler;
        private readonly HttpRequestMessage _request;
        private long _bytesReceived;
        private long? _totalBytesToReceive;
        private long _bytesSent;
        private long? _totalBytesToSend;

        public ProgressStream(
          Stream innerStream,
          ApizrProgressHandler handler,
          HttpRequestMessage request,
          HttpResponseMessage response)
          : base(innerStream)
        {
            if (request.Content != null)
                _totalBytesToSend = request.Content.Headers.ContentLength;
            if (response is {Content: { }})
                _totalBytesToReceive = response.Content.Headers.ContentLength;
            _handler = handler;
            _request = request;
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            var bytesReceived = InnerStream.Read(buffer, offset, count);
            ReportBytesReceived(bytesReceived, (object)null);
            return bytesReceived;
        }

        public override int ReadByte()
        {
            var num = InnerStream.ReadByte();
            ReportBytesReceived(num == -1 ? 0 : 1, (object)null);
            return num;
        }

        public override async Task<int> ReadAsync(
          byte[] buffer,
          int offset,
          int count,
          CancellationToken cancellationToken)
        {
            var progressStream = this;
            var bytesReceived = await progressStream.InnerStream.ReadAsync(buffer, offset, count, cancellationToken);
            progressStream.ReportBytesReceived(bytesReceived, (object)null);
            return bytesReceived;
        }

        public override IAsyncResult BeginRead(
          byte[] buffer,
          int offset,
          int count,
          AsyncCallback callback,
          object state)
        {
            return InnerStream.BeginRead(buffer, offset, count, callback, state);
        }

        public override int EndRead(IAsyncResult asyncResult)
        {
            var bytesReceived = InnerStream.EndRead(asyncResult);
            ReportBytesReceived(bytesReceived, asyncResult.AsyncState);
            return bytesReceived;
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            InnerStream.Write(buffer, offset, count);
            ReportBytesSent(count, (object)null);
        }

        public override void WriteByte(byte value)
        {
            InnerStream.WriteByte(value);
            ReportBytesSent(1, (object)null);
        }

        public override async Task WriteAsync(
          byte[] buffer,
          int offset,
          int count,
          CancellationToken cancellationToken)
        {
            var progressStream = this;
            await progressStream.InnerStream.WriteAsync(buffer, offset, count, cancellationToken);
            progressStream.ReportBytesSent(count, (object)null);
        }

        public override IAsyncResult BeginWrite(
          byte[] buffer,
          int offset,
          int count,
          AsyncCallback callback,
          object state)
        {
            return (IAsyncResult)new ProgressWriteAsyncResult(InnerStream, this, buffer, offset, count, callback, state);
        }

        public override void EndWrite(IAsyncResult asyncResult) => ProgressWriteAsyncResult.End(asyncResult);

        internal void ReportBytesSent(int bytesSent, object userState)
        {
            if (bytesSent <= 0)
                return;
            _bytesSent += (long)bytesSent;
            var progressPercentage = 0;
            if (_totalBytesToSend.HasValue)
            {
                var totalBytesToSend1 = _totalBytesToSend;
                long num1 = 0;
                if (!(totalBytesToSend1.GetValueOrDefault() == num1 & totalBytesToSend1.HasValue))
                {
                    var num2 = 100L * _bytesSent;
                    var totalBytesToSend2 = _totalBytesToSend;
                    progressPercentage = (int)(totalBytesToSend2.HasValue ? new long?(num2 / totalBytesToSend2.GetValueOrDefault()) : new long?()).Value;
                }
            }
            _handler.OnHttpRequestProgress(_request, new HttpProgressEventArgs(progressPercentage, userState, _bytesSent, _totalBytesToSend));
        }

        private void ReportBytesReceived(int bytesReceived, object userState)
        {
            if (bytesReceived <= 0)
                return;
            _bytesReceived += (long)bytesReceived;
            var progressPercentage = 0;
            if (_totalBytesToReceive.HasValue)
            {
                var totalBytesToReceive1 = _totalBytesToReceive;
                long num1 = 0;
                if (!(totalBytesToReceive1.GetValueOrDefault() == num1 & totalBytesToReceive1.HasValue))
                {
                    var num2 = 100L * _bytesReceived;
                    var totalBytesToReceive2 = _totalBytesToReceive;
                    progressPercentage = (int)(totalBytesToReceive2.HasValue ? new long?(num2 / totalBytesToReceive2.GetValueOrDefault()) : new long?()).Value;
                }
            }
            _handler.OnHttpResponseProgress(_request, new HttpProgressEventArgs(progressPercentage, userState, _bytesReceived, _totalBytesToReceive));
        }
    }

    /// <summary>
    /// Stream that delegates to inner stream.
    /// This is taken from System.Net.Http
    /// </summary>
    internal abstract class DelegatingStream : Stream
    {
        private readonly Stream _innerStream;

        protected DelegatingStream(Stream innerStream) => _innerStream = innerStream != null ? innerStream : throw new ArgumentNullException(nameof(innerStream));

        protected Stream InnerStream => _innerStream;

        public override bool CanRead => _innerStream.CanRead;

        public override bool CanSeek => _innerStream.CanSeek;

        public override bool CanWrite => _innerStream.CanWrite;

        public override long Length => _innerStream.Length;

        public override long Position
        {
            get => _innerStream.Position;
            set => _innerStream.Position = value;
        }

        public override int ReadTimeout
        {
            get => _innerStream.ReadTimeout;
            set => _innerStream.ReadTimeout = value;
        }

        public override bool CanTimeout => _innerStream.CanTimeout;

        public override int WriteTimeout
        {
            get => _innerStream.WriteTimeout;
            set => _innerStream.WriteTimeout = value;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                _innerStream.Dispose();
            base.Dispose(disposing);
        }

        public override long Seek(long offset, SeekOrigin origin) => _innerStream.Seek(offset, origin);

        public override int Read(byte[] buffer, int offset, int count) => _innerStream.Read(buffer, offset, count);

        public override Task<int> ReadAsync(
          byte[] buffer,
          int offset,
          int count,
          CancellationToken cancellationToken)
        {
            return _innerStream.ReadAsync(buffer, offset, count, cancellationToken);
        }

        public override IAsyncResult BeginRead(
          byte[] buffer,
          int offset,
          int count,
          AsyncCallback callback,
          object state)
        {
            return _innerStream.BeginRead(buffer, offset, count, callback, state);
        }

        public override int EndRead(IAsyncResult asyncResult) => _innerStream.EndRead(asyncResult);

        public override int ReadByte() => _innerStream.ReadByte();

        public override void Flush() => _innerStream.Flush();

        public override Task FlushAsync(CancellationToken cancellationToken) => _innerStream.FlushAsync(cancellationToken);

        public override void SetLength(long value) => _innerStream.SetLength(value);

        public override void Write(byte[] buffer, int offset, int count) => _innerStream.Write(buffer, offset, count);

        public override Task WriteAsync(
          byte[] buffer,
          int offset,
          int count,
          CancellationToken cancellationToken)
        {
            return _innerStream.WriteAsync(buffer, offset, count, cancellationToken);
        }

        public override IAsyncResult BeginWrite(
          byte[] buffer,
          int offset,
          int count,
          AsyncCallback callback,
          object state)
        {
            return _innerStream.BeginWrite(buffer, offset, count, callback, state);
        }

        public override void EndWrite(IAsyncResult asyncResult) => _innerStream.EndWrite(asyncResult);

        public override void WriteByte(byte value) => _innerStream.WriteByte(value);
    }

    internal class ProgressWriteAsyncResult : AsyncResult
    {
        private static readonly AsyncCallback _writeCompletedCallback = new AsyncCallback(WriteCompletedCallback);
        private readonly Stream _innerStream;
        private readonly ProgressStream _progressStream;
        private readonly int _count;

        public ProgressWriteAsyncResult(
            Stream innerStream,
            ProgressStream progressStream,
            byte[] buffer,
            int offset,
            int count,
            AsyncCallback callback,
            object state)
            : base(callback, state)
        {
            _innerStream = innerStream;
            _progressStream = progressStream;
            _count = count;
            try
            {
                var result = innerStream.BeginWrite(buffer, offset, count, _writeCompletedCallback, (object)this);
                if (!result.CompletedSynchronously)
                    return;
                WriteCompleted(result);
            }
            catch (Exception ex)
            {
                Complete(true, ex);
            }
        }

        private static void WriteCompletedCallback(IAsyncResult result)
        {
            if (result.CompletedSynchronously)
                return;
            var asyncState = (ProgressWriteAsyncResult)result.AsyncState;
            try
            {
                asyncState.WriteCompleted(result);
            }
            catch (Exception ex)
            {
                asyncState.Complete(false, ex);
            }
        }

        private void WriteCompleted(IAsyncResult result)
        {
            _innerStream.EndWrite(result);
            _progressStream.ReportBytesSent(_count, AsyncState);
            Complete(result.CompletedSynchronously);
        }

        public static void End(IAsyncResult result) => AsyncResult.End<ProgressWriteAsyncResult>(result);
    }

    internal abstract class AsyncResult : IAsyncResult
    {
        private AsyncCallback _callback;
        private object _state;
        private bool _isCompleted;
        private bool _completedSynchronously;
        private bool _endCalled;
        private Exception _exception;

        protected AsyncResult(AsyncCallback callback, object state)
        {
            _callback = callback;
            _state = state;
        }

        public object AsyncState => _state;

        public WaitHandle AsyncWaitHandle => (WaitHandle)null;

        public bool CompletedSynchronously => _completedSynchronously;

        public bool HasCallback => _callback != null;

        public bool IsCompleted => _isCompleted;

        protected void Complete(bool completedSynchronously)
        {
            if (_isCompleted)
                throw new InvalidOperationException(string.Format(FileTransferResources.AsyncResult_MultipleCompletes, (object)GetType().Name));
            _completedSynchronously = completedSynchronously;
            _isCompleted = true;
            if (_callback == null)
                return;
            try
            {
                _callback((IAsyncResult)this);
            }
            catch (Exception ex)
            {
                var callbackThrewException = FileTransferResources.AsyncResult_CallbackThrewException;
                var objArray = Array.Empty<object>();
                throw new InvalidOperationException(string.Format(callbackThrewException, objArray), ex);
            }
        }

        protected void Complete(bool completedSynchronously, Exception exception)
        {
            _exception = exception;
            Complete(completedSynchronously);
        }

        protected static TAsyncResult End<TAsyncResult>(IAsyncResult result) where TAsyncResult : AsyncResult
        {
            if (result == null)
                throw new ArgumentNullException(nameof(result));
            if (!(result is TAsyncResult asyncResult))
                throw new ArgumentNullException(nameof(result), FileTransferResources.AsyncResult_ResultMismatch);
            if (!asyncResult._isCompleted)
                asyncResult.AsyncWaitHandle.WaitOne();
            asyncResult._endCalled = !asyncResult._endCalled ? true : throw new ArgumentNullException(FileTransferResources.AsyncResult_MultipleEnds);
            return asyncResult._exception == null ? asyncResult : throw asyncResult._exception;
        }
    }

    internal static class HttpHeaderExtensions
    {
        public static void CopyTo(this HttpContentHeaders fromHeaders, HttpContentHeaders toHeaders)
        {
            foreach (var fromHeader in (HttpHeaders)fromHeaders)
                toHeaders.TryAddWithoutValidation(fromHeader.Key, fromHeader.Value);
        }
    }
}