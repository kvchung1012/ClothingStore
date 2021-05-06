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
    public class EmployeeService : IEmployeeService
    {
        private readonly ClothingStoreContext db = new ClothingStoreContext();

        public Task<bool> AddOrUpdate(Employee Employee)
        {
            throw new NotImplementedException();
        }



        public async Task<bool> DeleteById(int Id)
        {
            try
            {
                var target = await db.Employees.FindAsync(Id);
                db.Employees.Remove(target);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<List<Employee>> GetAll()
        {
            return await db.Employees.Where(x => x.IsDeleted == false).ToListAsync();
        }

        public async Task<ResponseData<Employee>> GetListData(RequestData requestData)
        {

            var list = await db.Employees.Where(x => x.IsDeleted == false).ToListAsync();

            //total record
            var totalCount = list.Count();

            //filtering
            if (requestData.ListFilter != null)
            {
                foreach (var item in requestData.ListFilter)
                {
                    list = list.Where(x => x.GetType().GetProperty(item.Key).PropertyType.Name == "String"
                    ? x.GetType().GetProperty(item.Key).GetValue(x).ToString().ToLower().Contains(item.Value)
                    : x.GetType().GetProperty(item.Key).GetValue(x).Equals(item.Value)).ToList();
                }

                totalCount = list.Count();
            }

            //sorting
            if (!string.IsNullOrEmpty(requestData.OrderBy))
            {
                if (requestData.IsAsc)
                    list = list.OrderBy(x => x.GetType().GetProperty(requestData.OrderBy).GetValue(x)).ToList();
                else
                    list = list.OrderByDescending(x => x.GetType().GetProperty(requestData.OrderBy).GetValue(x)).ToList();
            }

            //paginate
            requestData.PageSize = totalCount;
            
            if(requestData.PageNumber != 0)
            {
                list = list.Skip(requestData.PageSize * (requestData.PageNumber - 1)).Take(requestData.PageSize).ToList();
            }

            ResponseData<Employee> responseData = new ResponseData<Employee>()
            {
                Data = list,
                PageCount = totalCount % requestData.PageSize == 0 ? totalCount / requestData.PageSize : totalCount / requestData.PageSize + 1,
                PageNumber = requestData.PageNumber,
                PageSize = requestData.PageSize,
                OrderBy = requestData.OrderBy,
                IsAsc = requestData.IsAsc
            };

            return responseData;
        }

        public async Task<Employee> GetObjectById(int Id)
        {
            return await db.Employees.FindAsync(Id);
        }
    }
}
