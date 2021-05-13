﻿using ClothesStore.Model.Model.EF;
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
        readonly ClothingStoreContext db = new ClothingStoreContext();
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
                    pro.Slug = pro.Slug;
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
            foreach (var item in con)
            {
                if (item.Stock > 0)
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

        //Test Client
        public async Task<List<ProductAndProductConfigModelView>> GetListProductByQtyAndPosition(int pos, int qty)
        {
            List<ProductAndProductConfigModelView> listproducts = new List<ProductAndProductConfigModelView>();
            var products = await db.Products.OrderByDescending(x => x.Id).Skip(pos).Take(qty).ToListAsync();
            foreach (var item in products)
            {
                ProductAndProductConfigModelView p = new ProductAndProductConfigModelView()
                {
                    product = item,
                    configs = await GetListConfigProductByProductId(item.Id),
                    images = await GetListProductImageByProductId(item.Id)
                };
                listproducts.Add(p);
            };
            return listproducts;
        }

        public async Task<ProductAndProductConfigModelView> GetProductDetailById(int id)
        {
            var product = await db.Products.FindAsync(id);
            ProductAndProductConfigModelView p = new ProductAndProductConfigModelView()
            {
                product = product,
                configs = await GetListConfigProductByProductId(product.Id),
                images = await GetListProductImageByProductId(product.Id)
            };
            return p;
        }

        public async Task<List<ProductAndProductConfigModelView>> GetRelatedProduct(int id, int qty)
        {
            List<ProductAndProductConfigModelView> listproducts = new List<ProductAndProductConfigModelView>();
            var product = await db.Products.FindAsync(id);
            foreach (var item in await db.Products.ToListAsync())
            {
                if (product.CategoryId == item.CategoryId && product.Id != item.Id)
                {
                    ProductAndProductConfigModelView p = new ProductAndProductConfigModelView()
                    {
                        product = item,
                        configs = await GetListConfigProductByProductId(item.Id),
                        images = await GetListProductImageByProductId(item.Id)
                    };
                    listproducts.Add(p);
                }
            };
            return listproducts.Take(qty).ToList();

        }

        public async Task<List<ColorModelView>> GetColorConfig(int productId, int sizeId)
        {
            List<ColorModelView> data = new List<ColorModelView>();
            var colors = await db.ConfigProducts.Where(x => x.ProductId == productId && x.SizeId == sizeId).ToListAsync();
            foreach (var c in colors)
            {
                var obj = new ColorModelView()
                {
                    color = db.Colors.Find(c.ColorId),
                    config = c
                };
                data.Add(obj);
            }
            return data;
        }

        public async Task<List<ProductView>> GetListProduct(FilterProduct filterProduct, int pageSize)
        {
            try
            {
                List<ProductView> products = new List<ProductView>();
                var data = await db.Products.Where(x=>x.IsDeleted == false).ToListAsync();
                if (filterProduct.search != "" && filterProduct.search != null)
                {
                    data = data.Where(x => x.Name.Contains(filterProduct.search)).ToList();
                }
                if (filterProduct.categoryId != 0)
                {
                    data = await db.Products.Where(x => x.CategoryId == filterProduct.categoryId).ToListAsync();
                }
                if (filterProduct.brands != null)
                {
                    data = data.Where(x => filterProduct.brands.IndexOf((int)x.BrandId) >= 0).ToList();
                }
                if (filterProduct.colors != null)
                {
                    foreach (var item in data)
                    {
                        var configs = db.ConfigProducts.Where(x => x.ProductId == item.Id).ToList();
                        var check = configs.Where(x => filterProduct.colors.IndexOf((int)x.ColorId) >= 0).FirstOrDefault();
                        if (check != null)
                            data = data.Where(x => x.Id != check.ProductId).ToList();
                    }
                }

                foreach (var item in data)
                {
                    var config = db.ConfigProducts.Where(x => x.ProductId == item.Id).FirstOrDefault();
                    ProductView product = new ProductView()
                    {
                        product = item,
                        price = (double)(config == null ? 0 : config.Price)
                    };
                    products.Add(product);
                }
                if (pageSize != 0)
                    products = products.Take(pageSize).ToList();
                if (filterProduct.sort != 0)
                {
                    if (filterProduct.sort == 1)
                        products = products.OrderByDescending(x => x.product.CreatedDate).ToList();
                    else if (filterProduct.sort == 2)
                        products = products.OrderBy(x => x.price).ToList();
                    else
                        products = products.OrderByDescending(x => x.price).ToList();
                }
                return products;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<ProductOverView> GetInforProduct(int Id)
        {
            var product = db.Products.Find(Id);
            if (product != null)
            {
                ProductOverView data = new ProductOverView();
                List<Size> sizes = new List<Size>();
                List<ProductImage> images = new List<ProductImage>();
                images = await db.ProductImages.Where(x => x.ProductId == Id).ToListAsync();
                var config = await db.ConfigProducts.Where(x => x.ProductId == Id).ToListAsync();
                foreach (var item in config)
                {
                    Size size = db.Sizes.Find(item.SizeId);
                    if (size != null && sizes.Where(x => x.Id == size.Id).Count() == 0)
                        sizes.Add(size);
                }
                data.price = (double)(config.FirstOrDefault() == null ? 0 : config.FirstOrDefault().Price);
                data.product = product;
                data.size = sizes;
                data.images = images;
                return data;
            }
            return null;
        }


        public async Task<bool> Order(Order order, List<OrderDetail> orderDetails)
        {
            try
            {
                await db.Orders.AddAsync(order);
                await db.SaveChangesAsync();
                orderDetails = orderDetails.Select(x => { x.OrderId = order.Id; x.Status = true; x.IsDeleted = false; return x; }).ToList();
                await db.OrderDetails.AddRangeAsync(orderDetails);
                await db.SaveChangesAsync();
                return true;
            }
            catch(Exception e)
            {
                return false;
            }
        }
    }
}

