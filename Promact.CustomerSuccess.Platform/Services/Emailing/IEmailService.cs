using Promact.CustomerSuccess.Platform.Services.Dtos;

namespace Promact.CustomerSuccess.Platform.Services.Emailing
{
    public interface IEmailService
    {
        void SendEmail(EmailDto request);
        void SendEmailToStakeHolder(EmailToStakeHolderDto r);
    }
}
