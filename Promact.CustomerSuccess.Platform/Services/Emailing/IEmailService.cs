using Promact.CustomerSuccess.Platform.Services.Dtos;

namespace Promact.CustomerSuccess.Platform.Services.Emailing
{
    public interface IEmailService
    {
        /// <summary>
        /// Sends an email to stakeholders.
        /// </summary>
        /// <param name="request">The email details.</param>
        Task SendEmailToStakeHolder(EmailToStakeHolderDto request);

        /// <summary>
        /// Sends an email to multiple recipients.
        /// </summary>
        /// <param name="email">The MimeMessage representing the email.</param>
        Task SendEmailToMultipleReciever(MimeKit.MimeMessage email);

        /// <summary>
        /// Sends an email.
        /// </summary>
        /// <param name="request">The email details.</param>
        Task SendEmail(EmailDto request);
    }
}
