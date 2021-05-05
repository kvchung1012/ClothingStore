using System;
using System.Collections.Generic;
using System.Text;

namespace ClothesStore.Model.ModelView
{
    public class RequestData
    {
        public RequestData()
        {
            ListFilter = new List<Filter>();
            PageNumber = 0;
            PageSize = 1;
            OrderBy = "";
            IsAsc = true;
        }
        public List<Filter> ListFilter { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public string OrderBy { get; set; }
        public bool IsAsc { get; set; }
    }
}
