using System;
using System.Linq;
using System.Net.Http;
using Apizr.Requesting;

namespace Apizr.Configuring
{
    public class ApizrRequestMethod : IEquatable<ApizrRequestMethod>
    {
        private readonly int[] _groups;
        private readonly int _level;

        private ApizrRequestMethod(string method, string methodName, int[] groups, int level)
        {
            Method = method;
            MethodName = methodName;
            _groups = groups;
            _level = level;
        }

        public string Method { get; }

        public string MethodName { get; }

        public static ApizrRequestMethod HttpGet { get; } = new(HttpMethod.Get.Method, nameof(HttpGet), [1], 1);

        public static ApizrRequestMethod HttpPost { get; } = new(HttpMethod.Post.Method, nameof(HttpPost), [1], 1);

        public static ApizrRequestMethod HttpPut { get; } = new(HttpMethod.Put.Method, nameof(HttpPut), [1], 1);
#if NETSTANDARD2_1 || NET6_0_OR_GREATER
        public static ApizrRequestMethod HttpPatch { get; } = new(HttpMethod.Patch.Method, nameof(HttpPatch), [1], 1);
#endif
        public static ApizrRequestMethod HttpDelete { get; } = new(HttpMethod.Delete.Method, nameof(HttpDelete), [1], 1);

        public static ApizrRequestMethod HttpHead { get; } = new(HttpMethod.Head.Method, nameof(HttpHead), [1], 1);

        public static ApizrRequestMethod HttpOptions { get; } = new(HttpMethod.Options.Method, nameof(HttpOptions), [1], 1);

        public static ApizrRequestMethod AllHttp { get; } = new("AllHttp", nameof(AllHttp), [1], 2);

        public static ApizrRequestMethod CrudCreate { get; } = new(nameof(ICrudApi<ApizrRequestMethod, string, string, string>.Create), nameof(CrudCreate), [2], 1);

        public static ApizrRequestMethod CrudReadAll { get; } = new(nameof(ICrudApi<ApizrRequestMethod, string, string, string>.ReadAll), nameof(CrudReadAll), [2], 1);

        public static ApizrRequestMethod CrudRead { get; } = new(nameof(ICrudApi<ApizrRequestMethod, string, string, string>.Read), nameof(CrudRead), [2], 1);

        public static ApizrRequestMethod CrudUpdate { get; } = new(nameof(ICrudApi<ApizrRequestMethod, string, string, string>.Update), nameof(CrudUpdate), [2], 1);

        public static ApizrRequestMethod CrudDelete { get; } = new(nameof(ICrudApi<ApizrRequestMethod, string, string, string>.Delete), nameof(CrudDelete), [2], 1);

        public static ApizrRequestMethod CrudSafeCreate { get; } = new(nameof(ICrudApi<ApizrRequestMethod, string, string, string>.SafeCreate), nameof(CrudSafeCreate), [2], 1);

        public static ApizrRequestMethod CrudSafeReadAll { get; } = new(nameof(ICrudApi<ApizrRequestMethod, string, string, string>.SafeReadAll), nameof(CrudSafeReadAll), [2], 1);

        public static ApizrRequestMethod CrudSafeRead { get; } = new(nameof(ICrudApi<ApizrRequestMethod, string, string, string>.SafeRead), nameof(CrudSafeRead), [2], 1);

        public static ApizrRequestMethod CrudSafeUpdate { get; } = new(nameof(ICrudApi<ApizrRequestMethod, string, string, string>.SafeUpdate), nameof(CrudSafeUpdate), [2], 1);

        public static ApizrRequestMethod CrudSafeDelete { get; } = new(nameof(ICrudApi<ApizrRequestMethod, string, string, string>.SafeDelete), nameof(CrudSafeDelete), [2], 1);

        public static ApizrRequestMethod AllCrud { get; } = new("AllCrud", nameof(AllCrud), [2], 2);

        public static ApizrRequestMethod All { get; } = new("All", nameof(All), [1, 2], 3);

        public bool Equals(ApizrRequestMethod other)
        {
            if (other is null)
            {
                return false;
            }

            if (ReferenceEquals(Method, other.Method))
            {
                // Strings are static, so there is a good chance that two equal methods use the same reference
                // (unless they differ in case).
                return true;
            }

            return string.Equals(Method, other.Method, StringComparison.OrdinalIgnoreCase);
        }

        /// <inheritdoc />
        public override bool Equals(object obj) => Equals(obj as ApizrRequestMethod);

        private int _hashcode;
        /// <inheritdoc />
        public override int GetHashCode()
        {
            if (_hashcode == 0) 
                _hashcode = StringComparer.OrdinalIgnoreCase.GetHashCode(Method);

            return _hashcode;
        }

        public static bool operator ==(ApizrRequestMethod left, ApizrRequestMethod right) =>
            left is null || right is null ?
                ReferenceEquals(left, right) :
                left.Equals(right);

        public static bool operator !=(ApizrRequestMethod left, ApizrRequestMethod right) => !(left == right);

        public static bool operator >=(ApizrRequestMethod left, ApizrRequestMethod right)
        {
            if(left is null)
                return false;

            if(right is null)
                return true;

            return left.Equals(right) || 
                   (left._groups.Intersect(right._groups).Any() && 
                    left._level > right._level);
        }

        public static bool operator <=(ApizrRequestMethod left, ApizrRequestMethod right) => !(left >= right);

        public static bool TryParse(string method, out ApizrRequestMethod requestMethod)
        {
            requestMethod = null;

            if (string.IsNullOrWhiteSpace(method))
                return false;

            method = method.Trim();

            var requestMethods = new[]
            {
#if NETSTANDARD2_1 || NET6_0_OR_GREATER
                HttpPatch,
#endif
                HttpGet, HttpPost, HttpPut, HttpDelete, HttpHead, HttpOptions,
                CrudCreate, CrudReadAll, CrudRead, CrudUpdate, CrudDelete,
                CrudSafeCreate, CrudSafeReadAll, CrudSafeRead, CrudSafeUpdate, CrudSafeDelete,
                AllHttp, AllCrud, All
            };

            foreach (var rm in requestMethods)
            {
                if (!method.Equals(rm.MethodName, StringComparison.OrdinalIgnoreCase)) 
                    continue;

                requestMethod = rm;
                return true;
            }

            return false;
        }

        public override string ToString() => Method;
    }
}