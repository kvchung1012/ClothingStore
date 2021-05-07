using ClothesStore.Model.Model.EF;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClothesStore.Model.ModelView
{
    public class OrderFullModelView
    {
        public Order order { get; set; }
        public List<OrderDetailModelView> orderDetails { get; set; }
    }
}
