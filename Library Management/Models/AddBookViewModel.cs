using System.ComponentModel.DataAnnotations;

namespace Library_Management.Models
{
    public class AddBookViewModel
    {
        [Required(ErrorMessage = "Book title is required.")]
        [Display(Name = "Book Title")]
        public string? Title { get; set; }

        [Required(ErrorMessage = "ISBN is required.")]
        [Display(Name = "ISBN")]
        public string? ISBN { get; set; }

        [Display(Name = "Description")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Genre is required.")]
        [Display(Name = "Genre")]
        public string? Genre { get; set; }

        [Display(Name = "Published Date")]
        public DateTime? PublishedDate { get; set; }

        // BookItem-specific fields
        [Display(Name = "Cover Image URL")]
        public string? CoverImageUrl { get; set; }

        [Display(Name = "Condition")]
        public string? Condition { get; set; }

        [Display(Name = "Source")]
        public string? Source { get; set; }

        // Author-specific fields
        [Required(ErrorMessage = "Author name is required.")]
        [Display(Name = "Author")]
        public string? Author { get; set; }

        [Display(Name = "Author Profile Image URL")]
        public string? AuthorProfileImageUrl { get; set; }
    }
}
