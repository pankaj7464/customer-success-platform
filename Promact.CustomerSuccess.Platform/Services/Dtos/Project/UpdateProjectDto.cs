﻿using Promact.CustomerSuccess.Platform.Entities.Constants;
using System.ComponentModel.DataAnnotations;

namespace Promact.CustomerSuccess.Platform.Services.Dtos.Project
{
    public class UpdateProjectDto
    {
        [Required]
        [StringLength(128)]
        public required string Name { get; set; }
        public required string Description { get; set; }
        public ProjectStatus status { get; set; }
        public Guid ManagerId { get; set; }
    }
}
