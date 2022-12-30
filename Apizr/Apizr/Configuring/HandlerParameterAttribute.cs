using Refit;

namespace Apizr.Configuring
{
    public abstract class HandlerParameterAttribute : PropertyAttribute
    {
        protected HandlerParameterAttribute(string key, object value) : base(key)
        {
            Value = value;
        }
        
        public object Value { get; set; }
    }

    public abstract class CrudHandlerParameterAttribute : HandlerParameterAttribute
    {
        /// <inheritdoc />
        protected CrudHandlerParameterAttribute(string key, object value) : base(key, value)
        {
        }
    }

    public abstract class ReadAllHandlerParameterAttribute : CrudHandlerParameterAttribute
    {
        /// <inheritdoc />
        protected ReadAllHandlerParameterAttribute(string key, object value) : base(key, value)
        {
        }
    }

    public abstract class ReadHandlerParameterAttribute : CrudHandlerParameterAttribute
    {
        /// <inheritdoc />
        protected ReadHandlerParameterAttribute(string key, object value) : base(key, value)
        {
        }
    }

    public abstract class CreateHandlerParameterAttribute : CrudHandlerParameterAttribute
    {
        /// <inheritdoc />
        protected CreateHandlerParameterAttribute(string key, object value) : base(key, value)
        {
        }
    }

    public abstract class UpdateHandlerParameterAttribute : CrudHandlerParameterAttribute
    {
        /// <inheritdoc />
        protected UpdateHandlerParameterAttribute(string key, object value) : base(key, value)
        {
        }
    }

    public abstract class DeleteHandlerParameterAttribute : CrudHandlerParameterAttribute
    {
        /// <inheritdoc />
        protected DeleteHandlerParameterAttribute(string key, object value) : base(key, value)
        {
        }
    }
}
