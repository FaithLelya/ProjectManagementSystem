// Add this to your Models folder
namespace ProjectManagementSystem.Models
{
    public class ProjectResource
    {
        public int ResourceId { get; set; }
        public string ResourceName { get; set; }
        public int QuantityUsed { get; set; }
        public int ProjectId { get; set; }
    }
}