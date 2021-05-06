using ClothesStore.Model.Model.EF;
using ClothesStore.Model.ModelView;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ClothesStore.Service.IService
{
    public interface ICategoryService
    {
        public Task<List<Category>> GetAll();
        public Task<ResponseData<Category>> GetListData(RequestData requestData);
        public Task<Brand> GetObjectById(int Id);
        public Task<bool> AddOrUpdate(Category category);
        public Task<bool> DeleteById(int Id);
    }
}
