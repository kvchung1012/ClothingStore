using ClothesStore.Model.Model.EF;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClothesStore.Model.ModelView
{
    public class ProductModelView
    {
        public Product product { get; set; }
        public String brand { get; set; }
        public String category { get; set; }
    }
}
