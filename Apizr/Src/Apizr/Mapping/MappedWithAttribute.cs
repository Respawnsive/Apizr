using System;

namespace Apizr.Mapping
{
    /// <summary>
    /// Tells Apizr to map api request object with model object
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Parameter)]
    public class MappedWithAttribute : Attribute
    {
        /// <summary>
        /// Tells Apizr to map api response to a model response
        /// </summary>
        /// <param name="mappedWithType"></param>
        public MappedWithAttribute(Type mappedWithType)
        {
            MappedWithType = mappedWithType;
        }

        /// <summary>
        /// The model object to map with
        /// </summary>
        public Type MappedWithType { get; }
    }

    /// <summary>
    /// Tells Apizr to map api request object with model object
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Parameter)]
    public class MappedWithAttribute<TMappedWith> : MappedWithAttribute
    {
        /// <inheritdoc />
        public MappedWithAttribute() : base(typeof(TMappedWith))
        {
        }
    }
}
