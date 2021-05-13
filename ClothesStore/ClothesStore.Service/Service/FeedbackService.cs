using ClothesStore.Model.Model.EF;
using ClothesStore.Service.IService;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothesStore.Service.Service
{
    public class FeedbackService : IFeedbackService
    {
        private readonly ClothingStoreContext db = new ClothingStoreContext();

        public async Task<List<Feedback>> GetPendingFeedbacks()
        {
            return await db.Feedbacks.Where(x => x.Status == false).ToListAsync();
        }
    }
}
