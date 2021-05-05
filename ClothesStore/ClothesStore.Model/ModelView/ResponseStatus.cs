using System;
using System.Collections.Generic;
using System.Text;

namespace ClothesStore.Model.ModelView
{
    class ResponseStatus
    {
        public bool success { get; set; }
        public List<Filter> error { get; set; }
    }
}