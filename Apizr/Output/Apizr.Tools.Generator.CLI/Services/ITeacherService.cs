using System.Threading.Tasks;
using System.Collections.Generic;
using Refit;
using Apizr;

namespace Apizr.Tools.Generator.CLI
{
    [WebApi]
    public interface ITeacherService
    {
        /// <param name="top">The max number of records.</param>
        /// <param name="skip">The number of records to skip.</param>
        /// <param name="filter">A function that must evaluate to true for a record to be returned.</param>
        /// <param name="select">Specifies a subset of properties to return. Use a comma separated list.</param>
        /// <param name="orderby">Determines what values are used to order a collection of records.</param>
        /// <param name="expand">Use to add related query data.</param>
        /// <returns>Success</returns>
        [Get("teachers")]
        Task TeachersGETAsync([Query] object top, [Query] object skip, [Query] object filter, [Query] object select, [Query] object orderby, [Query] object expand);

        /// <param name="top">The max number of records.</param>
        /// <param name="skip">The number of records to skip.</param>
        /// <param name="filter">A function that must evaluate to true for a record to be returned.</param>
        /// <param name="select">Specifies a subset of properties to return. Use a comma separated list.</param>
        /// <param name="orderby">Determines what values are used to order a collection of records.</param>
        /// <param name="expand">Use to add related query data.</param>
        /// <returns>Success</returns>
        [Post("teachers")]
        Task TeachersPOSTAsync([Query] object top, [Query] object skip, [Query] object filter, [Query] object select, [Query] object orderby, [Query] object expand, [Body] Teacher body);

        /// <param name="top">The max number of records.</param>
        /// <param name="skip">The number of records to skip.</param>
        /// <param name="filter">A function that must evaluate to true for a record to be returned.</param>
        /// <param name="select">Specifies a subset of properties to return. Use a comma separated list.</param>
        /// <param name="orderby">Determines what values are used to order a collection of records.</param>
        /// <param name="expand">Use to add related query data.</param>
        /// <returns>Success</returns>
        [Get("teachers/{id}")]
        Task TeachersGET2Async(int id, [Query] object top, [Query] object skip, [Query] object filter, [Query] object select, [Query] object orderby, [Query] object expand);

        /// <param name="top">The max number of records.</param>
        /// <param name="skip">The number of records to skip.</param>
        /// <param name="filter">A function that must evaluate to true for a record to be returned.</param>
        /// <param name="select">Specifies a subset of properties to return. Use a comma separated list.</param>
        /// <param name="orderby">Determines what values are used to order a collection of records.</param>
        /// <param name="expand">Use to add related query data.</param>
        /// <returns>Success</returns>
        [Put("teachers/{id}")]
        Task TeachersPUTAsync(int id, [Query] object top, [Query] object skip, [Query] object filter, [Query] object select, [Query] object orderby, [Query] object expand, [Body] Teacher body);

        /// <param name="top">The max number of records.</param>
        /// <param name="skip">The number of records to skip.</param>
        /// <param name="filter">A function that must evaluate to true for a record to be returned.</param>
        /// <param name="select">Specifies a subset of properties to return. Use a comma separated list.</param>
        /// <param name="orderby">Determines what values are used to order a collection of records.</param>
        /// <param name="expand">Use to add related query data.</param>
        /// <returns>Success</returns>
        [Delete("teachers/{id}")]
        Task TeachersDELETEAsync(int id, [Query] object top, [Query] object skip, [Query] object filter, [Query] object select, [Query] object orderby, [Query] object expand);

    }
}