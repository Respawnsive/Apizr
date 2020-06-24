using System;

namespace Apizr.Requesting
{
    /// <summary>
    /// Tells Apizr to auto register an <see cref="IApizrManager{ICrudApi}"/> for this decorated class (only for IServiceCollection extensions registration)
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class CrudAttribute : Attribute
    {
        /// <summary>
        /// Define this specific object's crud base uri and key type
        /// </summary>
        /// <param name="baseUri">This specific object's crud base uri</param>
        /// <param name="keyType">This specific object's crud key type</param>
        public CrudAttribute(string baseUri, Type keyType)
        {
            BaseUri = baseUri;
            KeyType = keyType;
        }

        /// <summary>
        /// This specific object's crud base uri
        /// </summary>
        public string BaseUri { get; }

        /// <summary>
        /// This specific object's crud key type
        /// </summary>
        public Type KeyType { get; }
    }
}
