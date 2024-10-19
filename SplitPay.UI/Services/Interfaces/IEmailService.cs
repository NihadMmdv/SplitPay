using SplitPay.DAL.Models;

namespace SplitPay.UI.Services.Interfaces
{
    public interface IEmailService
    {
        Task SendEmail(string url,string toEmail);
        Task SendPaymentEmail(string url,string toEmail,Loan loan);
    }
}
