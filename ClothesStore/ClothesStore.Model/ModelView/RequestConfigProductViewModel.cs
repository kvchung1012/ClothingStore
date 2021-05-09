using System;
using System.Collections.Generic;
using System.Text;

namespace ClothesStore.Model.ModelView
{
    public class RequestConfigProductViewModel
    {
        public RequestConfigProductViewModel()
        {
            Resources = new List<int>();
            Size = "";
            Color = "";
            Price = 0;
            Stock = 0;
        }

        public List<int> Resources { get; set; }
        public string Size { get; set; }
        public string Color { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
    }
}
