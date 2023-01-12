using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System;
using System.ComponentModel;
using System.Net;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Diagnostics.Contracts;
using System.Diagnostics.CodeAnalysis;

namespace Apizr.Integrations.FileTransfer
{
    /// <summary>
    /// The <see cref="ApizrProgressMessageHandler"/> provides a mechanism for getting progress event notifications
    /// when sending and receiving data in connection with exchanging HTTP requests and responses.
    /// Register event handlers for the events <see cref="HttpSendProgress"/> and <see cref="HttpReceiveProgress"/>
    /// to see events for data being sent and received.
    /// </summary>
    public class ApizrProgressMessageHandler : DelegatingHandler
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApizrProgressMessageHandler"/> class.
        /// </summary>
        public ApizrProgressMessageHandler()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApizrProgressMessageHandler"/> class.
        /// </summary>
        /// <param name="innerHandler">The inner handler to which this handler submits requests.</param>
        public ApizrProgressMessageHandler(HttpMessageHandler innerHandler)
            : base(innerHandler)
        {
        }

        /// <summary>
        /// Occurs every time the client sending data is making progress.
        /// </summary>
        public event EventHandler<HttpProgressEventArgs> HttpSendProgress;

        /// <summary>
        /// Occurs every time the client receiving data is making progress.
        /// </summary>
        public event EventHandler<HttpProgressEventArgs> HttpReceiveProgress;

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            AddRequestProgress(request);
            HttpResponseMessage response = await base.SendAsync(request, cancellationToken);

            if (HttpReceiveProgress != null && response != null && response.Content != null)
            {
                cancellationToken.ThrowIfCancellationRequested();
                await AddResponseProgressAsync(request, response);
            }

            return response;
        }

        /// <summary>
        /// Raises the <see cref="HttpSendProgress"/> event.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="e">The <see cref="HttpProgressEventArgs"/> instance containing the event data.</param>
        protected internal virtual void OnHttpRequestProgress(HttpRequestMessage request, HttpProgressEventArgs e)
        {
            if (HttpSendProgress != null)
            {
                HttpSendProgress(request, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="HttpReceiveProgress"/> event.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="e">The <see cref="HttpProgressEventArgs"/> instance containing the event data.</param>
        protected internal virtual void OnHttpResponseProgress(HttpRequestMessage request, HttpProgressEventArgs e)
        {
            if (HttpReceiveProgress != null)
            {
                HttpReceiveProgress(request, e);
            }
        }

        private void AddRequestProgress(HttpRequestMessage request)
        {
            if (HttpSendProgress != null && request != null && request.Content != null)
            {
                HttpContent progressContent = new ProgressContent(request.Content, this, request);
                request.Content = progressContent;
            }
        }

        private async Task<HttpResponseMessage> AddResponseProgressAsync(HttpRequestMessage request, HttpResponseMessage response)
        {
            Stream stream = await response.Content.ReadAsStreamAsync();
            ProgressStream progressStream = new ProgressStream(stream, this, request, response);
            HttpContent progressContent = new StreamContent(progressStream);
            response.Content.Headers.CopyTo(progressContent.Headers);
            response.Content = progressContent;
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

    /// <summary>
    /// Provides data for the events generated by <see cref="ApizrProgressMessageHandler"/>.
    /// </summary>
    public class HttpProgressEventArgs : ProgressChangedEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HttpProgressEventArgs"/> with the parameters given.
        /// </summary>
        /// <param name="progressPercentage">The percent completed of the overall exchange.</param>
        /// <param name="userToken">Any user state provided as part of reading or writing the data.</param>
        /// <param name="bytesTransferred">The current number of bytes either received or sent.</param>
        /// <param name="totalBytes">The total number of bytes expected to be received or sent.</param>
        public HttpProgressEventArgs(int progressPercentage, object userToken, long bytesTransferred, long? totalBytes)
            : base(progressPercentage, userToken)
        {
            BytesTransferred = bytesTransferred;
            TotalBytes = totalBytes;
        }

        /// <summary>
        /// Gets the current number of bytes transferred.
        /// </summary>
        public long BytesTransferred { get; private set; }

        /// <summary>
        /// Gets the total number of expected bytes to be sent or received. If the number is not known then this is null.
        /// </summary>
        public long? TotalBytes { get; private set; }
    }

    /// <summary>
    /// Wraps an inner <see cref="HttpContent"/> in order to insert a <see cref="ProgressStream"/> on writing data.
    /// </summary>
    internal class ProgressContent : HttpContent
    {
        private readonly HttpContent _innerContent;
        private readonly ApizrProgressMessageHandler _handler;
        private readonly HttpRequestMessage _request;

        public ProgressContent(HttpContent innerContent, ApizrProgressMessageHandler handler, HttpRequestMessage request)
        {
            Contract.Assert(innerContent != null);
            Contract.Assert(handler != null);
            Contract.Assert(request != null);

            _innerContent = innerContent;
            _handler = handler;
            _request = request;

            innerContent.Headers.CopyTo(Headers);
        }

        protected override Task SerializeToStreamAsync(Stream stream, TransportContext context)
        {
            ProgressStream progressStream = new ProgressStream(stream, _handler, _request, response: null);
            return _innerContent.CopyToAsync(progressStream);
        }

        protected override bool TryComputeLength(out long length)
        {
            long? contentLength = _innerContent.Headers.ContentLength;
            if (contentLength.HasValue)
            {
                length = contentLength.Value;
                return true;
            }

            length = -1;
            return false;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
            {
                _innerContent.Dispose();
            }
        }
    }

    /// <summary>
    /// This implementation of <see cref="DelegatingStream"/> registers how much data has been
    /// read (received) versus written (sent) for a particular HTTP operation. The implementation
    /// is client side in that the total bytes to send is taken from the request and the total
    /// bytes to read is taken from the response. In a server side scenario, it would be the
    /// other way around (reading the request and writing the response).
    /// </summary>
    internal class ProgressStream : DelegatingStream
    {
        private readonly ApizrProgressMessageHandler _handler;
        private readonly HttpRequestMessage _request;

        private long _bytesReceived;
        private long? _totalBytesToReceive;

        private long _bytesSent;
        private long? _totalBytesToSend;

        public ProgressStream(Stream innerStream, ApizrProgressMessageHandler handler, HttpRequestMessage request, HttpResponseMessage response)
            : base(innerStream)
        {
            Contract.Assert(handler != null);
            Contract.Assert(request != null);

            if (request.Content != null)
            {
                _totalBytesToSend = request.Content.Headers.ContentLength;
            }

            if (response != null && response.Content != null)
            {
                _totalBytesToReceive = response.Content.Headers.ContentLength;
            }

            _handler = handler;
            _request = request;
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            int bytesRead = InnerStream.Read(buffer, offset, count);
            ReportBytesReceived(bytesRead, userState: null);
            return bytesRead;
        }

        public override int ReadByte()
        {
            int byteRead = InnerStream.ReadByte();
            ReportBytesReceived(byteRead == -1 ? 0 : 1, userState: null);
            return byteRead;
        }

        public override async Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        {
            int readCount = await InnerStream.ReadAsync(buffer, offset, count, cancellationToken);
            ReportBytesReceived(readCount, userState: null);
            return readCount;
        }
#if !NETFX_CORE // BeginX and EndX are not supported on streams in portable libraries
        public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        {
            return InnerStream.BeginRead(buffer, offset, count, callback, state);
        }

        public override int EndRead(IAsyncResult asyncResult)
        {
            int bytesRead = InnerStream.EndRead(asyncResult);
            ReportBytesReceived(bytesRead, asyncResult.AsyncState);
            return bytesRead;
        }
#endif

        public override void Write(byte[] buffer, int offset, int count)
        {
            InnerStream.Write(buffer, offset, count);
            ReportBytesSent(count, userState: null);
        }

        public override void WriteByte(byte value)
        {
            InnerStream.WriteByte(value);
            ReportBytesSent(1, userState: null);
        }

        public override async Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        {
            await InnerStream.WriteAsync(buffer, offset, count, cancellationToken);
            ReportBytesSent(count, userState: null);
        }

#if !NETFX_CORE // BeginX and EndX are not supported on streams in portable libraries
        public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        {
            return new ProgressWriteAsyncResult(InnerStream, this, buffer, offset, count, callback, state);
        }

        public override void EndWrite(IAsyncResult asyncResult)
        {
            ProgressWriteAsyncResult.End(asyncResult);
        }
#endif

        internal void ReportBytesSent(int bytesSent, object userState)
        {
            if (bytesSent > 0)
            {
                _bytesSent += bytesSent;
                int percentage = 0;
                if (_totalBytesToSend.HasValue && _totalBytesToSend != 0)
                {
                    percentage = (int)((100L * _bytesSent) / _totalBytesToSend);
                }

                // We only pass the request as it is guaranteed to be non-null (the response may be null)
                _handler.OnHttpRequestProgress(_request, new HttpProgressEventArgs(percentage, userState, _bytesSent, _totalBytesToSend));
            }
        }

        private void ReportBytesReceived(int bytesReceived, object userState)
        {
            if (bytesReceived > 0)
            {
                _bytesReceived += bytesReceived;
                int percentage = 0;
                if (_totalBytesToReceive.HasValue && _totalBytesToReceive != 0)
                {
                    percentage = (int)((100L * _bytesReceived) / _totalBytesToReceive);
                }

                // We only pass the request as it is guaranteed to be non-null (the response may be null)
                _handler.OnHttpResponseProgress(_request, new HttpProgressEventArgs(percentage, userState, _bytesReceived, _totalBytesToReceive));
            }
        }
    }

    internal class ProgressWriteAsyncResult : AsyncResult
    {
        private static readonly AsyncCallback _writeCompletedCallback = WriteCompletedCallback;

        private readonly Stream _innerStream;
        private readonly ProgressStream _progressStream;
        private readonly int _count;

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Exception is handled as part of IAsyncResult completion.")]
        public ProgressWriteAsyncResult(Stream innerStream, ProgressStream progressStream, byte[] buffer, int offset, int count, AsyncCallback callback, object state)
            : base(callback, state)
        {
            Contract.Assert(innerStream != null);
            Contract.Assert(progressStream != null);
            Contract.Assert(buffer != null);

            _innerStream = innerStream;
            _progressStream = progressStream;
            _count = count;

            try
            {
                IAsyncResult result = innerStream.BeginWrite(buffer, offset, count, _writeCompletedCallback, this);
                if (result.CompletedSynchronously)
                {
                    WriteCompleted(result);
                }
            }
            catch (Exception e)
            {
                Complete(true, e);
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Exception is handled as part of IAsyncResult completion.")]
        private static void WriteCompletedCallback(IAsyncResult result)
        {
            if (result.CompletedSynchronously)
            {
                return;
            }

            ProgressWriteAsyncResult thisPtr = (ProgressWriteAsyncResult)result.AsyncState;
            try
            {
                thisPtr.WriteCompleted(result);
            }
            catch (Exception e)
            {
                thisPtr.Complete(false, e);
            }
        }

        private void WriteCompleted(IAsyncResult result)
        {
            _innerStream.EndWrite(result);
            _progressStream.ReportBytesSent(_count, AsyncState);
            Complete(result.CompletedSynchronously);
        }

        public static void End(IAsyncResult result)
        {
            AsyncResult.End<ProgressWriteAsyncResult>(result);
        }
    }

    [SuppressMessage("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable", Justification = "_manualResetEvent is disposed in End<TAsyncResult>")]
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

        public object AsyncState
        {
            get { return _state; }
        }

        public WaitHandle AsyncWaitHandle
        {
            get
            {
                Contract.Assert(false, "AsyncWaitHandle is not supported -- use callbacks instead.");
                return null;
            }
        }

        public bool CompletedSynchronously
        {
            get { return _completedSynchronously; }
        }

        public bool HasCallback
        {
            get { return _callback != null; }
        }

        public bool IsCompleted
        {
            get { return _isCompleted; }
        }

        protected void Complete(bool completedSynchronously)
        {
            if (_isCompleted)
            {
                throw new InvalidOperationException(string.Format(FileTransferResources.AsyncResult_MultipleCompletes, GetType().Name));
            }

            _completedSynchronously = completedSynchronously;
            _isCompleted = true;

            if (_callback != null)
            {
                try
                {
                    _callback(this);
                }
                catch (Exception e)
                {
                    throw new InvalidOperationException(FileTransferResources.AsyncResult_CallbackThrewException, e);
                }
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
            {
                throw new ArgumentNullException("result");
            }

            TAsyncResult thisPtr = result as TAsyncResult;

            if (thisPtr == null)
            {
                throw new ArgumentNullException("result", FileTransferResources.AsyncResult_ResultMismatch);
            }

            if (!thisPtr._isCompleted)
            {
                thisPtr.AsyncWaitHandle.WaitOne();
            }

            if (thisPtr._endCalled)
            {
                throw new InvalidOperationException(FileTransferResources.AsyncResult_MultipleEnds);
            }

            thisPtr._endCalled = true;

            if (thisPtr._exception != null)
            {
                throw thisPtr._exception;
            }

            return thisPtr;
        }
    }

    /// <summary>
    /// Stream that delegates to inner stream.
    /// This is taken from System.Net.Http
    /// </summary>
    internal abstract class DelegatingStream : Stream
    {
        private readonly Stream _innerStream;

        protected DelegatingStream(Stream innerStream)
        {
            if (innerStream == null)
            {
                throw new ArgumentNullException("innerStream");
            }
            _innerStream = innerStream;
        }

        protected Stream InnerStream
        {
            get { return _innerStream; }
        }

        public override bool CanRead
        {
            get { return _innerStream.CanRead; }
        }

        public override bool CanSeek
        {
            get { return _innerStream.CanSeek; }
        }

        public override bool CanWrite
        {
            get { return _innerStream.CanWrite; }
        }

        public override long Length
        {
            get { return _innerStream.Length; }
        }

        public override long Position
        {
            get { return _innerStream.Position; }
            set { _innerStream.Position = value; }
        }

        public override int ReadTimeout
        {
            get { return _innerStream.ReadTimeout; }
            set { _innerStream.ReadTimeout = value; }
        }

        public override bool CanTimeout
        {
            get { return _innerStream.CanTimeout; }
        }

        public override int WriteTimeout
        {
            get { return _innerStream.WriteTimeout; }
            set { _innerStream.WriteTimeout = value; }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _innerStream.Dispose();
            }
            base.Dispose(disposing);
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return _innerStream.Seek(offset, origin);
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return _innerStream.Read(buffer, offset, count);
        }

        public override Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        {
            return _innerStream.ReadAsync(buffer, offset, count, cancellationToken);
        }

#if !NETFX_CORE // BeginX and EndX not supported on Streams in portable libraries
        public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        {
            return _innerStream.BeginRead(buffer, offset, count, callback, state);
        }

        public override int EndRead(IAsyncResult asyncResult)
        {
            return _innerStream.EndRead(asyncResult);
        }
#endif

        public override int ReadByte()
        {
            return _innerStream.ReadByte();
        }

        public override void Flush()
        {
            _innerStream.Flush();
        }

        public override Task FlushAsync(CancellationToken cancellationToken)
        {
            return _innerStream.FlushAsync(cancellationToken);
        }

        public override void SetLength(long value)
        {
            _innerStream.SetLength(value);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            _innerStream.Write(buffer, offset, count);
        }

        public override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        {
            return _innerStream.WriteAsync(buffer, offset, count, cancellationToken);
        }

#if !NETFX_CORE // BeginX and EndX not supported on Streams in portable libraries
        public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        {
            return _innerStream.BeginWrite(buffer, offset, count, callback, state);
        }

        public override void EndWrite(IAsyncResult asyncResult)
        {
            _innerStream.EndWrite(asyncResult);
        }
#endif

        public override void WriteByte(byte value)
        {
            _innerStream.WriteByte(value);
        }
    }

    internal static class HttpHeaderExtensions
    {
        public static void CopyTo(this HttpContentHeaders fromHeaders, HttpContentHeaders toHeaders)
        {
            Contract.Assert(fromHeaders != null, "fromHeaders cannot be null.");
            Contract.Assert(toHeaders != null, "toHeaders cannot be null.");

            foreach (KeyValuePair<string, IEnumerable<string>> header in fromHeaders)
            {
                toHeaders.TryAddWithoutValidation(header.Key, header.Value);
            }
        }
    }
}