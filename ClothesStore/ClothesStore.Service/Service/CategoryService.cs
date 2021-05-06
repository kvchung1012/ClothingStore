using ClothesStore.Model.Model.EF;
using ClothesStore.Model.ModelView;
using ClothesStore.Service.IService;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClothesStore.Service.Service
{
    public class CategoryService : ICategoryService
    {
        ClothingStoreContext db = new ClothingStoreContext();
        public Task<bool> AddOrUpdate(Category category)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteById(int Id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Category>> GetAll()
        {
            return await db.Categories.Where(x => x.IsDeleted == false).ToListAsync();
        }

        public Task<ResponseData<Category>> GetListData(RequestData requestData)
        {
            throw new NotImplementedException();
        }

        public Task<Category> GetObjectById(int Id)
        {
            throw new NotImplementedException();
        }
    }
}
