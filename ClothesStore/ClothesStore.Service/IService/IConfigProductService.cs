using ClothesStore.Model.Model.EF;
using ClothesStore.Model.ModelView;
using ClothesStore.Model.ModelView.ProductConfig;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ClothesStore.Service.IService
{
    public interface  IConfigProductService
    {
        public Task<ResponseConfig> GetPriceAndStock(RequestConfig requestConfig);
        public Task<decimal> GetMinimumPrice(int ProductId);
    }
}
