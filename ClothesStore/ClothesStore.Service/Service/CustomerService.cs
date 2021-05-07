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
    public class CustomerService : ICustomerService
    {
        ClothingStoreContext db = new ClothingStoreContext();
        public async Task<bool> AddOrUpdate(Customer customer)
        {
            try
            {
                if (customer.Id == 0)
                {
                    customer.CreatedDate = DateTime.Now;
                    customer.IsDeleted = false;
                    await db.Customers.AddAsync(customer);
                    await db.SaveChangesAsync();
                    return true;
                }
                else
                {
                    var pro = await db.Customers.FindAsync(customer.Id);
                    pro.Name = customer.Name;
                    pro.UpdatedBy = customer.UpdatedBy;
                    pro.Phone = customer.Phone;
                    pro.Email = customer.Email;
                    pro.Address = customer.Address;
                    //pro.OrderBy = customer.OrderBy;
                    pro.Status = customer.Status;
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
            var obj = await db.Customers.FindAsync(Id);
            if (obj != null)
            {
                obj.IsDeleted = true;
                await db.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<List<Customer>> GetAll()
        {
            return await db.Customers.Where(x => x.IsDeleted == false).ToListAsync();
        }

        public async Task<ResponseData<Customer>> GetListData(RequestData requestData)
        {
            var data = await db.Customers.Where(x => x.IsDeleted == false).ToListAsync();
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

            ResponseData<Customer> responseData = new ResponseData<Customer>()
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

        public async Task<Customer> GetObjectById(int Id)
        {
            return await db.Customers.FindAsync(Id);
        }
    }
}
