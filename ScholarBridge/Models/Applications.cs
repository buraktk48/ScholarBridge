namespace ScholarBridge.Models
{
    public class Applications
    {
        public int Id { get; set; }
        public int StudentId { get; set; } 
        public int ScholarshipId { get; set; } 
        public DateTime ApplyDate { get; set; } 
        public string Status { get; set; } 
    }
}
