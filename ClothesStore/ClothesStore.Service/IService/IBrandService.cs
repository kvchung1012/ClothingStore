using ClothesStore.Model.Model.EF;
using ClothesStore.Model.ModelView;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ClothesStore.Service.IService
{
    public interface IBrandService
    {
        public Task<List<Brand>> GetAll();
        public Task<ResponseData<Brand>> GetListData(RequestData requestData);
        public Task<Brand> GetObjectById(int Id);
        public Task<bool> AddOrUpdate(Brand brand);
        public Task<bool> DeleteById(int Id);
    }
}
