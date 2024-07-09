using System;

namespace Apizr.Mapping
{
    /// <summary>
    /// Tells Apizr to map api request object with model object
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Parameter)]
    public class MappedWithAttribute : Attribute
    {
        internal MappedWithAttribute(Type firstEntityType, Type secondEntityType)
        {
            FirstEntityType = firstEntityType;
            SecondEntityType = secondEntityType;
        }

        /// <summary>
        /// Tells Apizr to map api response to a model response
        /// </summary>
        /// <param name="secondEntityType"></param>
        public MappedWithAttribute(Type secondEntityType)
        {
            SecondEntityType = secondEntityType;
        }
        
        internal Type FirstEntityType { get; }

        /// <summary>
        /// The model object to map with
        /// </summary>
        public Type SecondEntityType { get; }
    }

    /// <summary>
    /// Tells Apizr to map api request object with model object
    /// </summary>
    public class MappedWithAttribute<TSecondEntityType> : MappedWithAttribute
    {
        /// <inheritdoc />
        public MappedWithAttribute() : base(typeof(TSecondEntityType))
        {
        }
    }
}
