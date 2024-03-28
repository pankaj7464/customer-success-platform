namespace Promact.CustomerSuccess.Platform.Services.Dtos.ProjectUpdate
{
    public class CreateUpdateProjectUpdateDto
    {
        public DateTime Date { get; set; }
        public string GeneralUpdate { get; set; }
        public Guid ProjectId { get; set; }
    }
}
