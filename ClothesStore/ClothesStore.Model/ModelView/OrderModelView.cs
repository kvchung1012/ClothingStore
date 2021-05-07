using ClothesStore.Model.Model.EF;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClothesStore.Model.ModelView
{
    public class OrderModelView
    {
        public Order order { get; set; }
        public Employee employee { get; set; }
        public Customer customer { get; set; }
        public List<OrderDetail> orderDetails { get; set; }
    }
}
