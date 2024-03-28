
using Promact.CustomerSuccess.Platform.Services.Dtos.ApprovedTeam;
using Promact.CustomerSuccess.Platform.Services.Dtos.AuditHistory;
using Promact.CustomerSuccess.Platform.Services.Dtos.ClientFeedback;
using Promact.CustomerSuccess.Platform.Services.Dtos.MeetingMinute;
using Promact.CustomerSuccess.Platform.Services.Dtos.ProjectBudget;
using Promact.CustomerSuccess.Platform.Services.Dtos.ProjectResource;
using Promact.CustomerSuccess.Platform.Services.Dtos.ProjectUpdate;
using Promact.CustomerSuccess.Platform.Services.Dtos.sprint;
using Promact.CustomerSuccess.Platform.Services.Dtos.Stakeholder;

namespace Promact.CustomerSuccess.Platform.Services.Emailing
{
    public static class Template
    {
        public static string css = $@"  <style>
            /* CSS styles can be added here to style the email content */
            body {{
                font-family: Arial, sans-serif;
                line-height: 1.6;
            }}

            .container {{
                margin: 0 auto;
                padding: 20px;
                border: 1px solid #ccc;
                border-radius: 5px;
            }}

            table {{
                width: 100%;
                border-collapse: collapse;
            }}

            th, td {{
                padding: 8px;
                text-align: left;
                border-bottom: 1px solid #ddd;
            }}
        </style>";

        public static string GetApproveTeamEmailBody(ApprovedTeamDto team, string action)
        {
            string body = $@"
    <!DOCTYPE html>
    <html lang=""en"">
    <head>
        <meta charset=""UTF-8"">
        <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
        <title>Team Information</title>
      {css}
    </head>
    <body>
        <div class=""container"">
            <h2>Team {action} Information</h2>
            <table>
                <tr>
                    <th>No of Resources</th>
                    <th>Phase No</th>
                    <th>Role</th>
                    <th>Duration</th>
                    <th>Availability</th>
                </tr>
                <tr>
                    <td>{team.NoOfResources}</td>
                    <td>{team.PhaseNo}</td>
                    <td>{team.Role}</td>
                    <td>{team.Duration}</td>
                    <td>{team.Availability}</td>
                </tr>
            </table>
        </div>
    </body>
    </html>";

            return body;
        }

        public static string GetAuditHistoryEmailBody(AuditHistoryDto auditHistory,string action)
        {
            string body = $@"
<!DOCTYPE html>
<html lang=""en"">
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>Audit History</title>
    <style>
        /* CSS styles can be added here to style the email content */
        body {{
            font-family: Arial, sans-serif;
            line-height: 1.6;
        }}

        .container {{
            max-width: 600px;
            margin: 0 auto;
            padding: 20px;
            border: 1px solid #ccc;
            border-radius: 5px;
        }}

        table {{
            width: 100%;
            border-collapse: collapse;
        }}

        th, td {{
            padding: 8px;
            text-align: left;
            border-bottom: 1px solid #ddd;
        }}
    </style>
</head>
<body>
    <div class=""container"">
        <h2>Audit History</h2>
        <table>
            <tr>
                <th>Date Of Audit</th>
                <th>Reviewed By</th>
                <th>Status</th>
                <th>Reviewed Section</th>
                <th>Comment or Queries</th>
                <th>Action Item</th>
            </tr>
            <tr>
                <td>{auditHistory.DateOfAudit}</td>
                <td>{auditHistory.ReviewedBy}</td>
                <td>{auditHistory.Status}</td>
                <td>{auditHistory.ReviewedSection}</td>
                <td>{auditHistory.CommentOrQueries}</td>
                <td>{auditHistory.ActionItem}</td>
            </tr>
        </table>
    </div>
</body>
</html>";

            return body;
        }

        public static string GetClientFeedbackEmailBody(ClientFeedbackDto clientFeedback, string action)
        {
            string body = $@"
<!DOCTYPE html>
<html lang=""en"">
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>Client Feedback</title>
    {css}
</head>
<body>
    <div class=""container"">
        <h2>Client Feedback {action}</h2>
        <table>
            <tr>
                <th>Feedback Date</th>
                <th>Feedback Type</th>
                <th>Details</th>
            </tr>
            <tr>
                <td>{clientFeedback.FeedbackDate}</td>
                <td>{clientFeedback.FeedbackType}</td>
                <td>{clientFeedback.Details}</td>
            </tr>
        </table>
    </div>
</body>
</html>";

            return body;
        }

        public static string GenerateMeetingMinutesEmailBody(MeetingMinuteDto meetingMinute,string action)
        {
            // HTML template for the email body
            string body = $@"
    <!DOCTYPE html>
    <html lang=""en"">
    <head>
        <meta charset=""UTF-8"">
        <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
        <title>Meeting Minutes</title>
       {css}
    </head>
    <body>
        <div class=""container"">
            <h2>Meeting Minutes {action}</h2>
            <table>
                <tr>
                    <th>Meeting Date</th>
                  
                    <th>MoM Link</th>
                    <th>Comments</th>
                </tr>
              
                <tr>
  <td>{meetingMinute.MeetingDate}</td>
 <td><a href=""{meetingMinute.MoMLink}"">{meetingMinute.MoMLink}</a></td>
                    <td>{meetingMinute.Comments}</td>
                </tr>
            </table>
        </div>
    </body>
    </html>";

            return body;
        }
        public static string GenerateClientFeedbackEmailBody(ClientFeedbackDto feedback,string action)
        {
            // HTML template for the email body
            string body = $@"
    <!DOCTYPE html>
    <html lang=""en"">
    <head>
        <meta charset=""UTF-8"">
        <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
        <title>Client Feedback</title>
        <style>
            /* CSS styles can be added here to style the email content */
            body {{
                font-family: Arial, sans-serif;
                line-height: 1.6;
            }}

            .container {{
                max-width: 600px;
                margin: 0 auto;
                padding: 20px;
                border: 1px solid #ccc;
                border-radius: 5px;
            }}

            table {{
                width: 100%;
                border-collapse: collapse;
            }}

            th, td {{
                padding: 8px;
                text-align: left;
                border-bottom: 1px solid #ddd;
            }}
        </style>
    </head>
    <body>
        <div class=""container"">
            <h2>Client Feedback {action}</h2>
            <table>
                <tr>
                    <th>Feedback Date</th>
                    <td>{feedback.FeedbackDate}</td>
                </tr>
                <tr>
                    <th>Feedback Type</th>
                    <td>{feedback.FeedbackType}</td>
                </tr>
                <tr>
                    <th>Details</th>
                    <td>{feedback.Details}</td>
                </tr>
            </table>
        </div>
    </body>
    </html>";

            return body;
        }

        public static string GenerateProjectBudgetEmailBody(ProjectBudgetDto projectBudget,string action)
        {
            // HTML template for the email body
            string body = $@"
    <!DOCTYPE html>
    <html lang=""en"">
    <head>
        <meta charset=""UTF-8"">
        <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
        <title>Project Budget</title>
        <style>
            /* CSS styles can be added here to style the email content */
            body {{
                font-family: Arial, sans-serif;
                line-height: 1.6;
            }}

            .container {{
                max-width: 600px;
                margin: 0 auto;
                padding: 20px;
                border: 1px solid #ccc;
                border-radius: 5px;
            }}

            table {{
                width: 100%;
                border-collapse: collapse;
            }}

            th, td {{
                padding: 8px;
                text-align: left;
                border-bottom: 1px solid #ddd;
            }}
        </style>
    </head>
    <body>
        <div class=""container"">
            <h2>Project Budget {action}</h2>
            <table>
                <tr>
                    <th>Project Type</th>
                    <td>{projectBudget.Type}</td>
                </tr>
                <tr>
                    <th>Duration (Months)</th>
                    <td>{projectBudget.DurationInMonths}</td>
                </tr>
                <tr>
                    <th>Contract Duration</th>
                    <td>{projectBudget.ContractDuration}</td>
                </tr>
                <tr>
                    <th>Budgeted Hours</th>
                    <td>{projectBudget.BudgetedHours}</td>
                </tr>
                <tr>
                    <th>Budgeted Cost</th>
                    <td>{projectBudget.BudgetedCost}</td>
                </tr>
                <tr>
                    <th>Currency</th>
                    <td>{projectBudget.Currency}</td>
                </tr>
            </table>
        </div>
    </body>
    </html>";

            return body;
        }
        public static string GenerateProjectResourceEmailBody(ProjectResourcesDto projectResource,string action)
        {
            // HTML template for the email body
            string body = $@"
    <!DOCTYPE html>
    <html lang=""en"">
    <head>
        <meta charset=""UTF-8"">
        <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
        <title>Project Resource {action}</title>
      {css}
    </head>
    <body>
        <div class=""container"">
            <h2>Project Resource {action}</h2>
            <table>
            
                <tr>
                    <th>Allocation Percentage</th>
                    <th>End Date</th>
                    <th>Start Date</th>
                    <th>Role</th>
                </tr>
                <tr>
                    <td>{projectResource.AllocationPercentage}</td>
                    <td>{projectResource.Start}</td>
                    <td>{projectResource.End}</td>
                    <td>{projectResource.Role}</td>
                </tr>
                
            </table>
        </div>
    </body>
    </html>";

            return body;
        }

        public static string GetProjectUpdateEmailBody(ProjectUpdateDto projectUpdate,string action)
        {
            string body = $@"
<!DOCTYPE html>
<html lang=""en"">
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>Project Update</title>
   {css}
</head>
<body>
    <div class=""container"">
        <h2>Project Update  {action}</h2>
        <table>
            <tr>
                <th>Date</th>
                <th>General Update</th>
             
            </tr>
            <tr>
                <td>{projectUpdate.Date}</td>
                <td>{projectUpdate.GeneralUpdate}</td>
            
            </tr>
        </table>
    </div>
</body>
</html>";

            return body;
        }


        public static string GetSprintEmailBody(SprintDto sprint,string action)
        {
            string body = $@"
<!DOCTYPE html>
<html lang=""en"">
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>Sprint Information</title>
    {css}
</head>
<body>
    <div class=""container"">
        <h2>Sprint Information {action}</h2>
        <table>
            <tr>
              
                <th>Start Date</th>
                <th>End Date</th>
                <th>Status</th>
                <th>Comments</th>
                <th>Goals</th>
                <th>Sprint Number</th>
            </tr>
            <tr>
               
                <td>{sprint.StartDate}</td>
                <td>{sprint.EndDate}</td>
                <td>{sprint.Status}</td>
                <td>{sprint.Comments}</td>
                <td>{sprint.Goals}</td>
                <td>{sprint.SprintNumber}</td>
            </tr>
        </table>
    </div>
</body>
</html>";

            return body;
        }

        public static string GetStakeholderEmailBody(StakeholderDto stakeholder,string action)
        {
            string body = $@"
<!DOCTYPE html>
<html lang=""en"">
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>Stakeholder Information</title>
   {css}
</head>
<body>
    <div class=""container"">
        <h2>Stakeholder Information {action}</h2>
        <table>
            <tr>
                <th>Title</th>
                <th>Name</th>
                <th>Email</th>
          
            </tr>
            <tr>
                <td>{stakeholder.Title}</td>
                <td>{stakeholder.Name}</td>
                <td>{stakeholder.Email}</td>
               
            </tr>
        </table>
    </div>
</body>
</html>";

            return body;
        }
        public static string GenerateConfirmationEmail( string userName, string userEmail, string confirmationLink)
        {
            string body = $@"
        <!DOCTYPE html>
        <html lang=""en"">
        <head>
            <meta charset=""UTF-8"">
            <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
            <title>New User Registered</title>
            <style>
                /* CSS styles can be added here to style the email content */
                body {{
                    font-family: Arial, sans-serif;
                    line-height: 1.6;
                    background-color: #f4f4f4;
                    margin: 0;
                    padding: 0;
                }}

                .container {{
                    max-width: 600px;
                    margin: 0 auto;
                    padding: 20px;
                    background: #ffffff;
                    border-radius: 5px;
                    box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
                }}

                .btn {{
                    display: inline-block;
                    background-color: #007bff;
                    color: #ffffff;
                    padding: 10px 20px;
                    text-decoration: none;
                    border-radius: 5px;
                }}

                .btn:hover {{
                    background-color: #0056b3;
                }}
            </style>
        </head>
        <body>
            <div class=""container"">
                <h2>New User Registered</h2>
                <p>A new user has been registered with the following details:</p>
                <ul>
                    <li><strong>Name:</strong> {userName}</li>
                    <li><strong>Email:</strong> {userEmail}</li>
                </ul>
                <p>Please confirm the registration by clicking the button below:</p>
                <a class=""btn"" href=""{confirmationLink}"">Confirm Registration</a>
            </div>
        </body>
        </html>";

            return body;
        }

        internal static string GetEmailTemplate(string name)
        {
            throw new NotImplementedException();
        }
    }
}
