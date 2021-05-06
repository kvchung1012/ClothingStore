using ClothesStore.Model.Model.EF;
using ClothesStore.Model.ModelView;
using ClothesStore.Service.IService;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothesStore.Service.Service
{
    public class ColorService : IColorService
    {

        ClothingStoreContext db = new ClothingStoreContext();
        public async Task<bool> AddOrUpdate(Color color)
        {
            try
            {
                if (color.Id == 0)
                {
                    color.CreatedDate = DateTime.Now;
                    color.IsDeleted = false;
                    await db.Colors.AddAsync(color);
                    await db.SaveChangesAsync();
                    return true;
                }
                else
                {
                    var pro = await db.Colors.FindAsync(color.Id);
                    pro.Name = color.Name;
                    pro.Value = color.Value;
                    pro.OrderBy = color.OrderBy;
                    pro.Status = color.Status;
                    //pro.IsDeleted = color.IsDeleted;
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
            var obj = await db.Colors.FindAsync(Id);
            if (obj != null)
            {
                obj.IsDeleted = true;
                await db.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<List<Color>> GetAll()
        {
            return await db.Colors.Where(x => x.IsDeleted == false).ToListAsync();
        }

        public async Task<ResponseData<Color>> GetListData(RequestData requestData)
        {
            var data = await db.Colors.Where(x => x.IsDeleted == false).ToListAsync();
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

            ResponseData<Color> responseData = new ResponseData<Color>()
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

        public async Task<Color> GetObjectById(int Id)
        {
            return await db.Colors.FindAsync(Id);
        }
    }
}
