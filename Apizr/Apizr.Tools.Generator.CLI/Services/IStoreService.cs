using System.Threading.Tasks;
using System.Collections.Generic;
using Refit;
using Apizr;

namespace Apizr.Tools.Generator.CLI
{
    [WebApi]
    public interface IStoreService
    {
        /// <summary>
        /// Place an order for a pet
        /// </summary>
        /// <param name="body">order placed for purchasing the pet</param>
        /// <returns>successful operation</returns>
        [Post("store/order")]
        Task<Order> PlaceOrderAsync([Body] Order body);

        /// <summary>
        /// Find purchase order by ID
        /// </summary>
        /// <param name="orderId">ID of pet that needs to be fetched</param>
        /// <returns>successful operation</returns>
        [Get("store/order/{orderId}")]
        Task<Order> GetOrderByIdAsync(long orderId);

        /// <summary>
        /// Delete purchase order by ID
        /// </summary>
        /// <param name="orderId">ID of the order that needs to be deleted</param>
        [Delete("store/order/{orderId}")]
        Task DeleteOrderAsync(long orderId);

        /// <summary>
        /// Returns pet inventories by status
        /// </summary>
        /// <returns>successful operation</returns>
        [Get("store/inventory")]
        Task<IDictionary<string,Int>> GetInventoryAsync();

    }
}