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
        public async Task<bool> AddOrUpdate(Category category)
        {
            try
            {
                if (category.Id == 0)
                {
                    category.CreatedDate = DateTime.Now;
                    category.IsDeleted = false;
                    await db.Categories.AddAsync(category);
                    await db.SaveChangesAsync();
                    return true;
                }
                else
                {
                    var pro = await db.Categories.FindAsync(category.Id);
                    pro.Name = category.Name;
                    pro.Slug = category.Slug;
                    pro.Image = category.Image;
                    pro.Description = category.Description;
                    pro.OrderBy = category.OrderBy;
                    pro.Status = category.Status;
                    pro.UpdatedDate = DateTime.Now;
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        public async Task<bool> DeleteById(int Id)
        {
            var obj = await db.Categories.FindAsync(Id);
            if (obj != null)
            {
                obj.IsDeleted = true;
                await db.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<List<Category>> GetAll()
        {
            return await db.Categories.Where(x => x.IsDeleted == false).ToListAsync();
        }

        public async Task<ResponseData<Category>> GetListData(RequestData requestData)
        {
            var data = await db.Categories.Where(x => x.IsDeleted == false).ToListAsync();
            // get total records
            var totalRecords = data.Count();
            // filter
            if (requestData.ListFilter != null)
            {
                foreach (var filter in requestData.ListFilter)
                {
                    data = data.Where(x => x.GetType().GetProperty(filter.Key).PropertyType.Name == "String" ? x.GetType().GetProperty(filter.Key).GetValue(x).ToString().Contains(filter.Value) : x.GetType().GetProperty(filter.Key).GetValue(x).Equals(filter.Value)).ToList();
                }
                totalRecords = data.Count();
            }

            // sort by
            if (!String.IsNullOrEmpty(requestData.OrderBy))
            {
                if (requestData.IsAsc)
                    data = data.OrderBy(x => x.GetType().GetProperty(requestData.OrderBy).GetValue(x)).ToList();
                else
                    data = data = data.OrderByDescending(x => x.GetType().GetProperty(requestData.OrderBy).GetValue(x)).ToList();
            }
            //pagination
            if (requestData.PageNumber != 0)
            {
                data = data.Skip(requestData.PageSize * (requestData.PageNumber - 1)).Take(requestData.PageSize).ToList();
            }

            ResponseData<Category> responseData = new ResponseData<Category>()
            {
                Data = data,
                PageCount = totalRecords % requestData.PageSize == 0 ? totalRecords / requestData.PageSize : totalRecords / requestData.PageSize + 1,
                PageNumber = requestData.PageNumber,
                PageSize = requestData.PageSize,
                OrderBy = requestData.OrderBy,
                IsAsc = requestData.IsAsc
            };
            return responseData;
        }

        public async Task<Category> GetObjectById(int Id)
        {
            return await db.Categories.FindAsync(Id);
        }
    }
}
