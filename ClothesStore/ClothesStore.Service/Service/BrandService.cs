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
    public class BrandService : IBrandService
    {
        ClothingStoreContext db = new ClothingStoreContext();
        public async Task<bool> AddOrUpdate(Brand brand)
        {
            try
            {
                if (brand.Id == 0)
                {
                    brand.CreatedDate = DateTime.Now;
                    brand.IsDeleted = false;
                    await db.Brands.AddAsync(brand);
                    await db.SaveChangesAsync();
                    return true;
                }
                else
                {
                    var pro = await db.Brands.FindAsync(brand.Id);
                    pro.Name = brand.Name;
                    pro.Slug = brand.Slug;
                    pro.Image = brand.Image;
                    pro.Description = brand.Description;
                    pro.OrderBy = brand.OrderBy;
                    pro.Status = brand.Status;
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
            var obj = await db.Brands.FindAsync(Id);
            if(obj != null)
            {
                obj.IsDeleted = true;
                await db.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<List<Brand>> GetAll()
        {
            return await db.Brands.Where(x => x.IsDeleted == false).ToListAsync();
        }

        public async Task<ResponseData<Brand>> GetListData(RequestData requestData)
        {
            var data = await db.Brands.Where(x => x.IsDeleted == false).ToListAsync();
            // get total records
            var totalRecords = data.Count();
            // filter
            if (requestData.ListFilter != null)
            {
                foreach (var filter in requestData.ListFilter)
                {
                    data = data.Where(x => x.GetType().GetProperty(filter.Key).PropertyType.Name == "String" ? x.GetType().GetProperty(filter.Key).GetValue(x).ToString().Contains(filter.Value): x.GetType().GetProperty(filter.Key).GetValue(x).Equals(filter.Value)).ToList();
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

            ResponseData<Brand> responseData = new ResponseData<Brand>()
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

        public async Task<Brand> GetObjectById(int Id)
        {
            return await db.Brands.FindAsync(Id);
        }
    }
}
