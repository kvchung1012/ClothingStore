using System;
using System.Collections.Generic;
using System.Text;

namespace ClothesStore.Model.ModelView
{
    public class FilterProduct
    {
        public List<int> colors { get; set; }
        public List<int> brands { get; set; }
        public int categoryId { get; set; }
        public int sort { get; set; }
        public string search { get; set; }
    }
}
