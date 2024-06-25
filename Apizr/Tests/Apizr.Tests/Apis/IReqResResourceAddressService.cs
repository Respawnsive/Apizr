using System.Threading.Tasks;
using Apizr.Configuring;
using Apizr.Logging;
using Apizr.Logging.Attributes;
using Apizr.Tests.Models;
using Refit;

//[assembly:Log]
namespace Apizr.Tests.Apis
{
    [BaseAddress("https://reqres.in/api"), Log(HttpMessageParts.None)]
    public interface IReqResResourceAddressService
    {
        [Get("/unknown")]
        Task<ApiResult<Resource>> GetResourcesAsync();
    }
}
