using ClothesStore.Model.Model.EF;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClothesStore.Model.ModelView
{
    public class ProductOverView
    {
        public Product product { get; set; }
        public double price { get; set; }
        public List<Size> size { get; set; }
        public List<ProductImage> images { get; set; }
    }
}
