using MailKit.Security;
using MimeKit.Text;
using MimeKit;
using MailKit.Net.Smtp;
using Promact.CustomerSuccess.Platform.Services.Dtos;
using Promact.CustomerSuccess.Platform.Entities;
using Volo.Abp.Domain.Repositories;

namespace Promact.CustomerSuccess.Platform.Services.Emailing
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly IRepository<Stakeholder, Guid> _stakeholderRepository;
        private readonly ILogger<EmailService> _logger;
        public EmailService(IConfiguration configuration, IRepository<Stakeholder, Guid> stakeholderRepository)
        {
            _configuration = configuration;
            _stakeholderRepository = stakeholderRepository;
        }

        public async void SendEmailToStakeHolder(EmailToStakeHolderDto request)
        {
            try
            {
                var stakeholders = await _stakeholderRepository.GetListAsync(s => s.ProjectId == request.ProjectId);
                var fromAddress = _configuration["EmailSettings:FromAddress"];
                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse(fromAddress));

                foreach (var stakeholder in stakeholders)
                {
                    email.To.Add(MailboxAddress.Parse(stakeholder.Email));
                    email.Body = new TextPart(TextFormat.Html)
                    {
                        Text = Template.GetEmailTemplate(stakeholder.Name)
                    };
                }
                email.Subject = request.Subject;

                // Send email asynchronously
                await Task.Run(() => SendEmailToMultipleReciever(email));
            }
            catch (Exception ex) { }
        }

        public void SendEmailToMultipleReciever(MimeMessage email)
        {

            Console.WriteLine("=================================================");
            var smtpServer = _configuration["EmailSettings:SmtpServer"];
            var port = int.Parse(_configuration["EmailSettings:Port"]);
            var username = _configuration["EmailSettings:Username"];
            var password = _configuration["EmailSettings:Password"];
            try
            {
                using (var client = new SmtpClient())
                {
                    client.Connect(smtpServer, port, SecureSocketOptions.StartTls);
                    client.Authenticate(username, password);
                    client.Send(email);
                    client.Disconnect(true);
                    _logger.LogInformation("email send Successfully");
                }

            }
            catch (SmtpCommandException ex)
            {
                // Log the specific SMTP command exception
                _logger.LogError("SMTP command error: {ex.Message}");
            }
            catch (SmtpProtocolException ex)
            {
                // Log the specific SMTP protocol exception
                _logger.LogError($"SMTP protocol error: {ex.Message}");
            }
            catch (AuthenticationException ex)
            {
                // Log the specific authentication exception
                _logger.LogError($"Authentication error: {ex.Message}");
            }
            catch (Exception ex)
            {
                // Log any other exceptions
                _logger.LogError($"Unexpected error: {ex.Message}");
            }

        }

        public void SendEmail(EmailDto request)
        {

            Console.WriteLine("=================================================");
            var smtpServer = _configuration["EmailSettings:SmtpServer"];
            var port = int.Parse(_configuration["EmailSettings:Port"]);
            var username = _configuration["EmailSettings:Username"];
            var password = _configuration["EmailSettings:Password"];
            var fromAddress = _configuration["EmailSettings:FromAddress"];
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(fromAddress));
            email.To.Add(MailboxAddress.Parse(request.To));
            email.Subject = request.Subject;
            email.Body = new TextPart(TextFormat.Html)
            {
                Text = request.Body
            };

            try
            {
                using (var client = new SmtpClient())
                {
                    client.Connect(smtpServer, port, SecureSocketOptions.StartTls);
                    client.Authenticate(username, password);
                    client.Send(email);
                    client.Disconnect(true);
                }

            }
            catch (Exception ex) { }

        }
    }
}