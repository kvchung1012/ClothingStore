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
    public class LoginService : ILoginService
    {
        private readonly ClothingStoreContext db = new ClothingStoreContext();

        public async Task<bool> ForgotPassword(string Email)
        {
            var list = await db.Employees.Where(x => x.Email == Email).ToListAsync();
            return list.Count() > 0;
        }

        public async Task<Employee> Login(string Email, string Password)
        {
            var list = await db.Employees.ToListAsync();
            var target = list.FirstOrDefault(x => x.Email == Email && x.Password == Password && x.Status == true && x.IsDeleted == false);
            return target;
        }
    }
}
