using System;

namespace Apizr.Mapping
{
    /// <summary>
    /// Tells Apizr to map api request object with model object
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class MappedWithAttribute : Attribute
    {
        internal MappedWithAttribute(Type sourceEntityType, Type targetEntityType)
        {
            SourceEntityType = sourceEntityType;
            TargetEntityType = targetEntityType;
        }

        /// <summary>
        /// Tells Apizr to map api response to a model response
        /// </summary>
        /// <param name="targetEntityType"></param>
        public MappedWithAttribute(Type targetEntityType)
        {
            TargetEntityType = targetEntityType;
        }
        
        internal Type SourceEntityType { get; }

        /// <summary>
        /// The model object to map with
        /// </summary>
        public Type TargetEntityType { get; }
    }

    /// <summary>
    /// Tells Apizr to map api request object with model object
    /// </summary>
    public class MappedWithAttribute<TTargetEntityType> : MappedWithAttribute
    {
        /// <inheritdoc />
        public MappedWithAttribute() : base(typeof(TTargetEntityType))
        {
        }
    }
}
