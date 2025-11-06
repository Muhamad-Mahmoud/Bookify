using System.Threading.Tasks;

namespace Bookify.BL.Services
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            // TODO: Implement actual email sending logic
            return Task.CompletedTask;
        }
    }
}