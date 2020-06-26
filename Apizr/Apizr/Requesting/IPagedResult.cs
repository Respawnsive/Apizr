using System.Collections.Generic;

namespace Apizr.Requesting
{
    public interface IPagedResult<T> where T : class
    {
        int Page { get; set; }
        int PerPage { get; set; }
        int Total { get; set; }
        int TotalPages { get; set; }
        IEnumerable<T> Data { get; set; }
    }
}