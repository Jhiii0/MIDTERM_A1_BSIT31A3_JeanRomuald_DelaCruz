using System.ComponentModel.DataAnnotations;

namespace Library_Management.Models
{
    public class PulloutBookCopyViewModel
    {
        public Guid BookCopyId { get; set; }
        public string? BookTitle { get; set; }
        public string? CoverImageUrl { get; set; }

        [Required(ErrorMessage = "Pullout reason is required.")]
        [Display(Name = "Reason for Pullout")]
        public string? PulloutReason { get; set; }
    }
}
