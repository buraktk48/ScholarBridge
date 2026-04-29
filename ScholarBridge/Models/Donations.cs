namespace ScholarBridge.Models
{
    public class Donations
    {
        public int Id { get; set; }
        public int DonorId { get; set; } 
        public decimal Amount { get; set; } 
        public DateTime Date { get; set; }
    }
}
}
