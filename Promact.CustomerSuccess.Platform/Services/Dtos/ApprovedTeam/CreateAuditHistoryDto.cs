﻿using Promact.CustomerSuccess.Platform.Entities.Constants;

namespace Promact.CustomerSuccess.Platform.Services.Dtos.ApprovedTeam
{
    public class CreateAuditHistoryDto
    {
        public DateTime DateOfAudit { get; set; }
        public Guid ReviewerId { get; set; }
        public SprintStatus Status { get; set; }
        public string ReviewedSection { get; set; }
        public string? CommentOrQueries { get; set; }
        public string? ActionItem { get; set; }
        public required Guid ProjectId { get; set; }
    }
}