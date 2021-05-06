using ClothesStore.Model.Model.EF;
using ClothesStore.Model.ModelView;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ClothesStore.Service.IService
{
    public interface ICustomerService
    {
        public Task<List<Customer>> GetAll();
        public Task<ResponseData<Customer>> GetListData(RequestData requestData);
        public Task<Customer> GetObjectById(int Id);
        public Task<bool> AddOrUpdate(Customer customer);
        public Task<bool> DeleteById(int Id);
    }
}
