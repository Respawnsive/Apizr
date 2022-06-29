using System.Threading.Tasks;
using System.Collections.Generic;
using Refit;
using Apizr;

namespace Apizr.Tools.Generator.CLI
{
    [WebApi]
    public interface ICourseService
    {
        /// <param name="top">The max number of records.</param>
        /// <param name="skip">The number of records to skip.</param>
        /// <param name="filter">A function that must evaluate to true for a record to be returned.</param>
        /// <param name="select">Specifies a subset of properties to return. Use a comma separated list.</param>
        /// <param name="orderby">Determines what values are used to order a collection of records.</param>
        /// <param name="expand">Use to add related query data.</param>
        /// <returns>Success</returns>
        [Get("courses")]
        Task CoursesGETAsync([Query] object top, [Query] object skip, [Query] object filter, [Query] object select, [Query] object orderby, [Query] object expand);

        /// <param name="top">The max number of records.</param>
        /// <param name="skip">The number of records to skip.</param>
        /// <param name="filter">A function that must evaluate to true for a record to be returned.</param>
        /// <param name="select">Specifies a subset of properties to return. Use a comma separated list.</param>
        /// <param name="orderby">Determines what values are used to order a collection of records.</param>
        /// <param name="expand">Use to add related query data.</param>
        /// <returns>Success</returns>
        [Post("courses")]
        Task CoursesPOSTAsync([Query] object top, [Query] object skip, [Query] object filter, [Query] object select, [Query] object orderby, [Query] object expand, [Body] Course body);

        /// <param name="top">The max number of records.</param>
        /// <param name="skip">The number of records to skip.</param>
        /// <param name="filter">A function that must evaluate to true for a record to be returned.</param>
        /// <param name="select">Specifies a subset of properties to return. Use a comma separated list.</param>
        /// <param name="orderby">Determines what values are used to order a collection of records.</param>
        /// <param name="expand">Use to add related query data.</param>
        /// <returns>Success</returns>
        [Get("courses/{id}")]
        Task CoursesGET2Async(int id, [Query] object top, [Query] object skip, [Query] object filter, [Query] object select, [Query] object orderby, [Query] object expand);

        /// <param name="top">The max number of records.</param>
        /// <param name="skip">The number of records to skip.</param>
        /// <param name="filter">A function that must evaluate to true for a record to be returned.</param>
        /// <param name="select">Specifies a subset of properties to return. Use a comma separated list.</param>
        /// <param name="orderby">Determines what values are used to order a collection of records.</param>
        /// <param name="expand">Use to add related query data.</param>
        /// <returns>Success</returns>
        [Put("courses/{id}")]
        Task CoursesPUTAsync(int id, [Query] object top, [Query] object skip, [Query] object filter, [Query] object select, [Query] object orderby, [Query] object expand, [Body] Course body);

        /// <param name="top">The max number of records.</param>
        /// <param name="skip">The number of records to skip.</param>
        /// <param name="filter">A function that must evaluate to true for a record to be returned.</param>
        /// <param name="select">Specifies a subset of properties to return. Use a comma separated list.</param>
        /// <param name="orderby">Determines what values are used to order a collection of records.</param>
        /// <param name="expand">Use to add related query data.</param>
        /// <returns>Success</returns>
        [Delete("courses/{id}")]
        Task CoursesDELETEAsync(int id, [Query] object top, [Query] object skip, [Query] object filter, [Query] object select, [Query] object orderby, [Query] object expand);

    }
}