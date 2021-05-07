using ClothesStore.Model.Model.EF;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClothesStore.Model.ModelView
{
    public class OrderDetailModelView
    {
        public OrderDetail orderDetail { get; set; }
        public Product product { get; set; }
        public string color { get; set; }
        public string size { get; set; }
    }
}
