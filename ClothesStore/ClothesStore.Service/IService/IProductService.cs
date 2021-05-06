using ClothesStore.Model.Model.EF;
using ClothesStore.Model.ModelView;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ClothesStore.Service.IService
{
    public interface IProductService
    {
        public Task<List<Product>> GetAll();
        public Task<ResponseData<Product>> GetListData(RequestData requestData);
        public Task<Product> GetObjectById(int Id);
        public Task<bool> AddOrUpdate(Product product,List<ConfigProduct> configProducts,List<ProductImage> productImage);
        public Task<bool> DeleteById(int Id);

        // define 
        public Task<List<Color>> GetAllColor();
        public Task<List<Size>> GetAllSize();
    }
}
