using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using Refit;
using Apizr;

namespace Apizr.Tools.Generator.CLI
{
    [WebApi]
    public interface IPetService
    {
        /// <summary>
        /// uploads an image
        /// </summary>
        /// <param name="petId">ID of pet to update</param>
        /// <param name="additionalMetadata">Additional data to pass to server</param>
        /// <param name="file">file to upload</param>
        /// <returns>successful operation</returns>
        [Post("pet/{petId}/uploadImage")]
        Task<ApiResponse> UploadFileAsync(long petId, string additionalMetadata, FileParameter file);

        /// <summary>
        /// Add a new pet to the store
        /// </summary>
        /// <param name="body">Pet object that needs to be added to the store</param>
        [Post("pet")]
        Task AddPetAsync([Body] Pet body);

        /// <summary>
        /// Update an existing pet
        /// </summary>
        /// <param name="body">Pet object that needs to be added to the store</param>
        [Put("pet")]
        Task UpdatePetAsync([Body] Pet body);

        /// <summary>
        /// Finds Pets by status
        /// </summary>
        /// <param name="status">Status values that need to be considered for filter</param>
        /// <returns>successful operation</returns>
        [Get("pet/findByStatus")]
        Task<ICollection<Pet>> FindPetsByStatusAsync([Query] IEnumerable<Anonymous> status);

        /// <summary>
        /// Finds Pets by tags
        /// </summary>
        /// <param name="tags">Tags to filter by</param>
        /// <returns>successful operation</returns>
        [Obsolete]
        [Get("pet/findByTags")]
        Task<ICollection<Pet>> FindPetsByTagsAsync([Query] IEnumerable<string> tags);

        /// <summary>
        /// Find pet by ID
        /// </summary>
        /// <param name="petId">ID of pet to return</param>
        /// <returns>successful operation</returns>
        [Get("pet/{petId}")]
        Task<Pet> GetPetByIdAsync(long petId);

        /// <summary>
        /// Updates a pet in the store with form data
        /// </summary>
        /// <param name="petId">ID of pet that needs to be updated</param>
        /// <param name="name">Updated name of the pet</param>
        /// <param name="status">Updated status of the pet</param>
        [Post("pet/{petId}")]
        Task UpdatePetWithFormAsync(long petId, string name, string status);

        /// <summary>
        /// Deletes a pet
        /// </summary>
        /// <param name="petId">Pet id to delete</param>
        [Delete("pet/{petId}")]
        [Headers("api_key")] 
        Task DeletePetAsync(long petId);

    }
}