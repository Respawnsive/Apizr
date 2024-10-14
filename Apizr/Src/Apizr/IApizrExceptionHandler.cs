using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Apizr
{
    public interface IApizrExceptionHandler
    {
        Task<bool> HandleAsync(ApizrException ex);
    }
}
