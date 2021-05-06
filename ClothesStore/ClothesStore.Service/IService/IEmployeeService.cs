using ClothesStore.Model.Model.EF;
using ClothesStore.Model.ModelView;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ClothesStore.Service.IService
{
    public interface IEmployeeService
    {
        public Task<List<Employee>> GetAll();
        public Task<ResponseData<Employee>> GetListData(RequestData requestData);
        public Task<Employee> GetObjectById(int Id);
        public Task<bool> AddOrUpdate(Employee Employee);
        public Task<bool> DeleteById(int Id);
    }
}
