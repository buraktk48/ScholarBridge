namespace ScholarBridge.Models
{
    public class Scholarships
    {
        public int Id { get; set; }
        public string Title { get; set; } 
        public string Description { get; set; } 
        public decimal Amount { get; set; } 
        public DateTime Deadline { get; set; } 
        public int OrganizationId { get; set; } 
    }
}
