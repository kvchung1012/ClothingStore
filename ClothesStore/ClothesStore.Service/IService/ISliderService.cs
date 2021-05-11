using ClothesStore.Model.Model.EF;
using ClothesStore.Model.ModelView;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ClothesStore.Service.IService
{
    public interface ISliderService
    {
        public Task<List<Slider>> GetAll();
        public Task<ResponseData<Slider>> GetListData(RequestData requestData);
        public Task<Slider> GetObjectById(int Id);
        public Task<bool> AddOrUpdate(Slider customer);
        public Task<bool> DeleteById(int Id);
    }
}
