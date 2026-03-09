using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BikeHub.Shared.Common
{
    public class PagedResult<T>
    {
        public int TotalRecords { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        
        public int TotalPages => (int)Math.Ceiling((double)TotalRecords / PageSize);

        public bool HasNextPage => Page < TotalPages;
        public bool HasPrevPage => Page > 1;

        public List<T> Data { get; set; } = new();

        public PagedResult(int TotalRecords, int Page, int PageSize, List<T> Data)
        {
            this.TotalRecords = TotalRecords;
            this.Page = Page;
            this.PageSize = PageSize;
            this.Data = Data;
        }


    }
}