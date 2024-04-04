using Promact.CustomerSuccess.Platform.Services.Dtos;

namespace Promact.CustomerSuccess.Platform.Services.Emailing
{
    public interface IEmailService
    {
        Task SendEmail(EmailDto request);
        Task SendEmailToStakeHolder(EmailToStakeHolderDto r);
    }
}
