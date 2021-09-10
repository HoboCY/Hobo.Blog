using System.Threading.Tasks;

namespace Blog.Service.Mails
{
    public interface IEmailService
    {
        Task SendEmailAsync(string email, string subject, string htmlMessage);
    }
}
