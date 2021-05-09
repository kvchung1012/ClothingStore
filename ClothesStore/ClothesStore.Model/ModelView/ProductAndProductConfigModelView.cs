using ClothesStore.Model.Model.EF;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClothesStore.Model.ModelView
{
    public class ProductAndProductConfigModelView
    {
        public Product product { get; set; }
        public List<ConfigProduct> configs { get; set; }

        public List<ProductImage> images { get; set; }
    }
}
