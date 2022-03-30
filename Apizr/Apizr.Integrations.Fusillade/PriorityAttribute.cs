using Refit;

namespace Apizr
{
    public class PriorityAttribute : PropertyAttribute
    {

        public PriorityAttribute() : base(Constants.PriorityKey)
        {
            
        }
    }
}
