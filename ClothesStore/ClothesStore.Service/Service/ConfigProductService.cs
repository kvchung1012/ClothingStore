using ClothesStore.Model.Model.EF;
using ClothesStore.Model.ModelView;
using ClothesStore.Model.ModelView.ProductConfig;
using ClothesStore.Service.IService;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace ClothesStore.Service.Service
{
    public class ConfigProductService : IConfigProductService
    {
        private readonly ClothingStoreContext db = new ClothingStoreContext();

        public async Task<decimal> GetMinimumPrice(int ProductId)
        {
            decimal? tmpPrice = await db.ConfigProducts.Where(x => x.ProductId == ProductId).MinAsync(x => x.Price);
            decimal price = tmpPrice ?? 0;
            return price;
        }

        public async Task<ResponseConfig> GetPriceAndStock(RequestConfig rq)
        {
            ResponseConfig res = new ResponseConfig();
            var data = await db.ConfigProducts.FirstOrDefaultAsync(x => x.ProductId == rq.ProductId && x.ColorId == rq.Color && x.SizeId == rq.Size);
            
            if (data != null)
            {
                res.Stock = data.Stock.HasValue ? data.Stock.Value : 0;
                res.Price = data.Price.HasValue ? data.Price.Value : 0;
            }
            else
            {
                res = null;
            }

            return res;
        }

    }
}
