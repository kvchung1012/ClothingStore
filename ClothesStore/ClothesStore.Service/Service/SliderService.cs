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
    public class SliderService : ISliderService
    {
        ClothingStoreContext db = new ClothingStoreContext();
        public async Task<bool> AddOrUpdate(Slider slider)
        {
            try
            {
                if (slider.Id == 0)
                {
                    slider.CreatedDate = DateTime.Now;
                    slider.IsDeleted = false;
                    await db.Sliders.AddAsync(slider);
                    await db.SaveChangesAsync();
                    return true;
                }
                else
                {
                    var pro = await db.Sliders.FindAsync(slider.Id);
                    pro.Name = slider.Name;
                    pro.UpdatedBy = slider.UpdatedBy;
                    pro.SubName = slider.SubName;
                    pro.Link = slider.Link;
                    pro.Image = slider.Image;
                    //pro.OrderBy = slider.OrderBy;
                    pro.Status = slider.Status;
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
            var obj = await db.Sliders.FindAsync(Id);
            if (obj != null)
            {
                obj.IsDeleted = true;
                await db.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<List<Slider>> GetAll()
        {
            return await db.Sliders.Where(x => x.IsDeleted == false).ToListAsync();
        }

        public async Task<ResponseData<Slider>> GetListData(RequestData requestData)
        {
            var data = await db.Sliders.Where(x => x.IsDeleted == false).ToListAsync();
            // get total records
            var totalRecords = data.Count();
            // filter
            if (requestData.ListFilter != null)
            {
                foreach (var filter in requestData.ListFilter)
                {
                    data = data.Where(x => x.GetType().GetProperty(filter.Key).PropertyType.Name == "String" ? x.GetType().GetProperty(filter.Key).GetValue(x).ToString().Contains(filter.Value) : x.GetType().GetProperty(filter.Key).GetValue(x).ToString().Equals(filter.Value)).ToList();
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

            ResponseData<Slider> responseData = new ResponseData<Slider>()
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

        public async Task<Slider> GetObjectById(int Id)
        {
            return await db.Sliders.FindAsync(Id);
        }
    }
}
