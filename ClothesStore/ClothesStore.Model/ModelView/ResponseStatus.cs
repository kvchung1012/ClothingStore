using System;
using System.Collections.Generic;
using System.Text;

namespace ClothesStore.Model.ModelView
{
    public class ResponseStatus
    {
        public bool success { get; set; }
        public List<Filter> error { get; set; }
    }
}