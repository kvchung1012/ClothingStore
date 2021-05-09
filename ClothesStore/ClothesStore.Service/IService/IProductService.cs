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
        public Task<ResponseData<ProductModelView>> GetListData(RequestData requestData);
        public Task<Product> GetObjectById(int Id);
        public Task<bool> AddOrUpdate(Product product,List<ConfigProduct> configProducts,List<ProductImage> productImage);
        public Task<bool> DeleteById(int Id);

        // get selected
        public Task<List<ProductImage>> GetListProductImageByProductId(int Id);
        public Task<List<ConfigProduct>> GetListConfigProductByProductId(int Id);

        public Task<List<ConfigProductModelView>> GetConfigProductDetail(int Id);

        //client
        public Task<List<ProductAndProductConfigModelView>> GetListProductByQtyAndPosition(int pos, int qty);
        public Task<ProductAndProductConfigModelView> GetProductDetailById(int id);
        public Task<List<ProductAndProductConfigModelView>> GetRelatedProduct(int id,int qty);


        // define 
        public Task<List<Color>> GetAllColor();
        public Task<List<Size>> GetAllSize();

    }
}
