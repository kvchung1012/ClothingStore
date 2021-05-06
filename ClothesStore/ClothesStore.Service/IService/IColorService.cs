using ClothesStore.Model.Model.EF;
using ClothesStore.Model.ModelView;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ClothesStore.Service.IService
{
    public interface IColorService
    {
        public Task<List<Color>> GetAll();
        public Task<ResponseData<Color>> GetListData(RequestData requestData);
        public Task<Color> GetObjectById(int Id);
        public Task<bool> AddOrUpdate(Color brand);
        public Task<bool> DeleteById(int Id);
    }
}
