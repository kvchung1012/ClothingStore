using ClothesStore.Model.Model.EF;
using ClothesStore.Model.ModelView;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ClothesStore.Service.IService
{
    public interface IFeedbackService
    {
        public Task<List<Feedback>> GetPendingFeedbacks();
        /*public Task<ResponseData<Feedback>> GetListData(RequestData requestData);
        public Task<Feedback> GetObjectById(int Id);
        public Task<bool> AddOrUpdate(Feedback brand);
        public Task<bool> DeleteById(int Id);*/
    }
}
