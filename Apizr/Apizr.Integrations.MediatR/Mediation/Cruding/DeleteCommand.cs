using Apizr.Mediation.Commanding;

namespace Apizr.Mediation.Cruding
{
    public class DeleteCommand<TKey> : ICommand
    {
        public DeleteCommand(TKey key)
        {
            Key = key;
        }

        public TKey Key { get; }
    }
}