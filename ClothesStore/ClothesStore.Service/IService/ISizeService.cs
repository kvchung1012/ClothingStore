using ClothesStore.Model.Model.EF;
using ClothesStore.Model.ModelView;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ClothesStore.Service.IService
{
    public interface ISizeService
    {
        public Task<List<Size>> GetAll();
        public Task<ResponseData<Size>> GetListData(RequestData requestData);
        public Task<Size> GetObjectById(int Id);
        public Task<bool> AddOrUpdate(Size size);
        public Task<bool> DeleteById(int Id);
    }
}
