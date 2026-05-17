using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ScholarBridge.Models
{
    public partial class StudentFile
    {
        [Key]
        public int FileId { get; set; }

        public int UserId { get; set; }

        public string? FileName { get; set; } 
        public string? FilePath { get; set; }

        [ForeignKey("UserId")]
        public virtual StudentDetail? Student { get; set; }
    }
}
