using ClothesStore.Model.Model.EF;
using ClothesStore.Model.ModelView;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ClothesStore.Service.IService
{
    public interface IOrderService
    {
        public Task<ResponseData<OrderModelView>> GetListData(RequestData requestData);
        public Task<OrderModelView> GetObjectById(int Id);
        public Task<OrderFullModelView> GetOrderById(int Id);
        public Task<bool> AddOrUpdate(Order order,List<OrderDetail> orderDetails);
        public Task<bool> DeleteById(int Id);
    }
}
