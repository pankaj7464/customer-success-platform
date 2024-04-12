namespace Promact.CustomerSuccess.Platform.Services.Dtos.ProjectResource
{
    public class CreateUpdateProjectResourceDto
    {
        public Guid ProjectId { get; set; }
        public double AllocationPercentage { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Guid RoleId { get; set; }
    }
}
