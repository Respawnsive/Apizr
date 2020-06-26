namespace Apizr.Mediation.Commanding
{
    public abstract class CommandBase<TPayload, TResponse> : ICommand<TPayload, TResponse>
    {
        protected CommandBase(TPayload payload)
        {
            Payload = payload;
        }

        public TPayload Payload { get; }
    }

    public abstract class CommandBase<TPayload> : ICommand<TPayload>
    {
        protected CommandBase(TPayload payload)
        {
            Payload = payload;
        }

        public TPayload Payload { get; }
    }
}
