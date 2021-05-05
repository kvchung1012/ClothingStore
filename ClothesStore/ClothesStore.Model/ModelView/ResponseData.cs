using System;
using System.Collections.Generic;
using System.Text;

namespace ClothesStore.Model.ModelView
{
    public class ResponseData<T>
    {
        public List<T> Data { get; set; }
        public int PageCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string OrderBy { get; set; }
        public bool IsAsc { get; set; }
    }
}
