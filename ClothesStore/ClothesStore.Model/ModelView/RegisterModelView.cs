using System;
using System.Collections.Generic;
using System.Text;

namespace ClothesStore.Model.ModelView
{
    public class RegisterModelView
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public DateTime? BirthDay { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmedPassword { get; set; }
    }
}
