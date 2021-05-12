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

        public async Task<Customer> CustomerHasUser(string Email, string Phone)
        {
            return await db.Customers.FirstOrDefaultAsync(x => x.Email == Email && x.Phone == Phone);
        }

        public async Task<Customer> CustomerLogin(string Email, string Password)
        {
            var list = await db.Customers.ToListAsync();
            var target = list.Where(x => x.Email == Email && x.Password == Password && x.IsDeleted == false).FirstOrDefault();
            return target;
        }

        public async Task<Employee> HasUser(string Email, string phone)
        {
            return await db.Employees.FirstOrDefaultAsync(x => x.Email == Email && x.Phone == phone);
        }

        public async Task<Employee> Login(string Email, string Password)
        {
            var list = await db.Employees.ToListAsync();
            var target = list.FirstOrDefault(x => x.Email == Email && x.Password == Password && x.IsDeleted == false);
            return target;
        }
    }
}
