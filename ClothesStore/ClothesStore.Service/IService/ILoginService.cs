using ClothesStore.Model.Model.EF;
using ClothesStore.Model.ModelView;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ClothesStore.Service.IService
{
    public interface ILoginService
    {
        public  Task<Employee> Login(string Email, string Password);
        public  Task<Employee> HasUser(Filter filter);
        public Task<Employee> HasUser(string Email, string Phone);
        public Task<Customer> CustomerLogin(string Email, string Password);
        public Task<Customer> CustomerHasUser(string Email, string Phone);
    }
}
