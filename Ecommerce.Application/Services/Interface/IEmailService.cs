﻿
namespace Ecommerce.Application.Services.Interface
{
    public interface IEmailService
    {
        void SendEmail(string toEmail, string subject, string body);
    }
}
