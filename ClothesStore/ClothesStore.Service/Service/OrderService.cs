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
    public class OrderService : IOrderService
    {
        ClothingStoreContext db = new ClothingStoreContext();
        public async Task<bool> AddOrUpdate(Order order, List<OrderDetail> orderDetails)
        {
            try
            {
                if (order.Id == 0)
                {
                    order.CreatedDate = DateTime.Now;
                    order.IsDeleted = false;
                    await db.Orders.AddAsync(order);
                    await db.SaveChangesAsync();

                    if (orderDetails != null)
                    {
                        orderDetails = orderDetails.Select(x => { x.OrderId = order.Id; x.Status = true; x.IsDeleted = false; return x; }).ToList();
                        db.OrderDetails.AddRange(orderDetails);
                        await db.SaveChangesAsync();
                    }
                    return true;
                }
                else
                {
                    var pro = await db.Orders.FindAsync(order.Id);
                    pro.CustomerId = order.CustomerId;
                    pro.EmployeeId = order.EmployeeId;
                    pro.Note = order.Note;
                    pro.Address = order.Address;
                    pro.Status = order.Status;
                    pro.UpdatedDate = DateTime.Now;
                    db.SaveChanges();
                    // cập nhật config product, images
                    db.OrderDetails.RemoveRange(db.OrderDetails.Where(x => x.OrderId == order.Id).ToList());
                    if (orderDetails != null)
                    {
                        orderDetails = orderDetails.Select(x => { x.OrderId = order.Id; x.Status = true; x.IsDeleted = false; return x; }).ToList();
                        db.OrderDetails.AddRange(orderDetails);
                        await db.SaveChangesAsync();
                    }
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
            var obj = await db.Orders.FindAsync(Id);
            obj.IsDeleted = true;
            await db.SaveChangesAsync();
            return true;
        }

        public async Task<ResponseData<OrderModelView>> GetListData(RequestData requestData)
        {
            List<OrderModelView> list = new List<OrderModelView>();
            var data = await db.Orders.Where(x => x.IsDeleted == false).ToListAsync();
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

            foreach (var item in data)
            {
                OrderModelView p = new OrderModelView()
                {
                    order = item,
                    customer = (await db.Customers.FindAsync(int.Parse(item.CustomerId.ToString()))),
                    employee = (await db.Employees.FindAsync(int.Parse(item.EmployeeId.ToString())))
                };
                list.Add(p);
            }
            ResponseData<OrderModelView> responseData = new ResponseData<OrderModelView>()
            {
                Data = list,
                PageCount = totalRecords % requestData.PageSize == 0 ? totalRecords / requestData.PageSize : totalRecords / requestData.PageSize + 1,
                PageNumber = requestData.PageNumber,
                PageSize = requestData.PageSize,
                OrderBy = requestData.OrderBy,
                IsAsc = requestData.IsAsc
            };
            return responseData;
        }

        public async Task<OrderModelView> GetObjectById(int Id)
        {
            var obj = await db.Orders.FindAsync(Id);
            OrderModelView p = new OrderModelView()
            {
                order = obj,
                customer = (await db.Customers.FindAsync(obj.CustomerId)),
                employee = (await db.Employees.FindAsync(obj.EmployeeId)),
            };
            return p;
        }

        public async Task<OrderFullModelView> GetOrderById(int Id)
        {
            var obj = await db.Orders.FindAsync(Id);
            OrderFullModelView data = new OrderFullModelView();
            List<OrderDetailModelView> details = new List<OrderDetailModelView>();
            var orderDetails = await db.OrderDetails.Where(x => x.OrderId == Id).ToListAsync();
            foreach(var item in orderDetails)
            {
                var config =  db.ConfigProducts.Where(x => x.Id == item.ConfigProductId).First();
                OrderDetailModelView detail = new OrderDetailModelView()
                {
                    orderDetail = item,
                    color = db.Colors.Find(config.ColorId).Value,
                    size = db.Sizes.Find(config.SizeId).Name,
                    product = db.Products.Find(config.ProductId),
                };
                details.Add(detail);
            }
            data.order = obj;
            data.orderDetails = details;
            return data;
        }
    }
}
