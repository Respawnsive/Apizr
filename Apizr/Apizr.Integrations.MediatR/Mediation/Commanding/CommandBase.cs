using Apizr.Mediation.Requesting.Base;
using Fusillade;
using MediatR;

namespace Apizr.Mediation.Commanding
{
    public abstract class CommandBase<TPayload, TResponse> : RequestBase<TResponse>, ICommand<TPayload, TResponse>
    {
        protected CommandBase(Priority priority = Priority.UserInitiated) : base(priority)
        {
            
        }
    }

    public abstract class CommandBase<TPayload> : RequestBase<Unit>, ICommand<TPayload>
    {
        protected CommandBase(Priority priority = Priority.UserInitiated) : base(priority)
        {

        }
    }

    public abstract class CommandBase : RequestBase<Unit>, ICommand
    {
        protected CommandBase(Priority priority = Priority.UserInitiated) : base(priority)
        {

        }
    }
}
