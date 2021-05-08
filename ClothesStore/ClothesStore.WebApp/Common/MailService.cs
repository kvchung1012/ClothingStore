using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace ClothesStore.WebApp.Common
{
    public static class MailService
    {
        public static void SendEmail(string email, string pass)
        {
            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = true,
                Credentials = new NetworkCredential("haidv2801@gmail.com", ""),
            };
            var mes = new MailMessage("haidv2801@gmail.com", email)
            {
                Subject = "Send to you password of ClothingStore",
                Body = "Mật khẩu mới của bạn là : " + pass,
            };
            smtp.Send(mes);
        }
    }
}
