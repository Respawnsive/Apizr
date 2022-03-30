using Refit;

namespace Apizr.Policing
{
    public class ContextAttribute : PropertyAttribute
    {
        public ContextAttribute() : base("PollyExecutionContext")
        {

        }
    }
}
