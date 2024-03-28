namespace Promact.CustomerSuccess.Platform.Services.Dtos.ProjectResource
{
    public class CreateUpdateProjectResourceDto
    {
        public Guid ProjectId { get; set; }
        public double AllocationPercentage { get; set; }
        public DateTime StartData { get; set; }
        public DateTime EndDate { get; set; }
        public required string Role { get; set; }
    }
}
