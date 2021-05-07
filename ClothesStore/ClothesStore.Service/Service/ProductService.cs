using ClothesStore.Model.Model.EF;
using ClothesStore.Model.ModelView;
using ClothesStore.Service.IService;
using Microsoft.AspNetCore.Session;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClothesStore.Service.Service
{
    public class ProductService : IProductService
    {
        ClothingStoreContext db = new ClothingStoreContext();
        public async Task<bool> AddOrUpdate(Product product, List<ConfigProduct> configProducts, List<ProductImage> productImage)
        {
            try
            {
                if (product.Id == 0)
                {
                    product.CreatedDate = DateTime.Now;
                    product.IsDeleted = false;
                    await db.Products.AddAsync(product);
                    await db.SaveChangesAsync();

                    if (configProducts != null)
                    {
                        configProducts = configProducts.Select(x => { x.ProductId = product.Id; x.Status = true; x.IsDeleted = false; return x; }).ToList();
                        db.ConfigProducts.AddRange(configProducts);
                        await db.SaveChangesAsync();
                    }
                    if (productImage != null)
                    {
                        productImage = productImage.Select(x => { x.ProductId = product.Id; return x; }).ToList();
                        db.ProductImages.AddRange(productImage);
                        db.SaveChanges();
                    }
                    return true;
                }
                else
                {
                    var pro = await db.Products.FindAsync(product.Id);
                    pro.BrandId = product.BrandId;
                    pro.UpdatedBy = product.UpdatedBy;
                    pro.CategoryId = product.CategoryId;
                    pro.Content = product.Content;
                    pro.Description = product.Description;
                    pro.Name = product.Name;
                    pro.Image = product.Image;
                    pro.OrderBy = product.OrderBy;
                    pro.Status = product.Status;
                    pro.UpdatedDate = DateTime.Now;
                    db.SaveChanges();

                    // cập nhật config product, images
                    db.ConfigProducts.RemoveRange(db.ConfigProducts.Where(x => x.ProductId == product.Id).ToList());
                    db.ProductImages.RemoveRange(db.ProductImages.Where(x => x.ProductId == product.Id).ToList());

                    if (configProducts != null)
                    {
                        configProducts = configProducts.Select(x => { x.ProductId = product.Id; x.Status = true; x.IsDeleted = false; return x; }).ToList();
                        db.ConfigProducts.AddRange(configProducts);
                        await db.SaveChangesAsync();
                    }
                    if (productImage != null)
                    {
                        productImage = productImage.Select(x => { x.ProductId = product.Id; return x; }).ToList();
                        db.ProductImages.AddRange(productImage);
                        db.SaveChanges();
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
            var obj = await db.Products.FindAsync(Id);
            if (obj == null)
                return false;
            else
            {
                obj.IsDeleted = true;
                await db.SaveChangesAsync();
                return true;
            }
        }

        public async Task<List<Product>> GetAll()
        {
            return await db.Products.Where(x => x.IsDeleted == false).ToListAsync();
        }

        public async Task<List<Color>> GetAllColor()
        {
            return await db.Colors.Where(x => x.IsDeleted == false).ToListAsync();
        }

        public async Task<List<Size>> GetAllSize()
        {
            return await db.Sizes.Where(x => x.IsDeleted == false).ToListAsync();
        }

        public async Task<List<ConfigProductModelView>> GetConfigProductDetail(int Id)
        {
            List<ConfigProductModelView> data = new List<ConfigProductModelView>();
            var con = await db.ConfigProducts.Where(x => x.ProductId == Id && x.IsDeleted == false).ToListAsync();
            foreach(var item in con)
            {
                if(item.Stock > 0)
                {
                    ConfigProductModelView c = new ConfigProductModelView()
                    {
                        config = item,
                        color = await db.Colors.FindAsync(item.ColorId),
                        size = await db.Sizes.FindAsync(item.SizeId)
                    };
                    data.Add(c);
                }
            }
            return data;
        }

        public async Task<List<ConfigProduct>> GetListConfigProductByProductId(int Id)
        {
            return await db.ConfigProducts.Where(x => x.ProductId == Id && x.IsDeleted == false).ToListAsync();
        }

      

        public async Task<ResponseData<ProductModelView>> GetListData(RequestData requestData)
        {
            List<ProductModelView> list = new List<ProductModelView>();
            var data = await db.Products.Where(x => x.IsDeleted == false).ToListAsync();
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

            foreach (var item in data)
            {
                ProductModelView p = new ProductModelView()
                {
                    product = item,
                    brand = (await db.Brands.FindAsync(item.BrandId)).Name,
                    category = (await db.Categories.FindAsync(item.CategoryId)).Name
                };
                list.Add(p);
            }
            ResponseData<ProductModelView> responseData = new ResponseData<ProductModelView>()
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

        public async Task<List<ProductImage>> GetListProductImageByProductId(int Id)
        {
            return await db.ProductImages.Where(x => x.ProductId == Id).ToListAsync();
        }

        public async Task<Product> GetObjectById(int Id)
        {
            return await db.Products.FindAsync(Id);
        }
    }
}
