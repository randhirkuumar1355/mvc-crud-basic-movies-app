using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace MovieApplication.ViewModels
{
    public class MovieVM
    {
        [Key]
        public int MovieId { get; set; }

        [Required]
        [StringLength(35, ErrorMessage = "Movie name length can't be more than 35.", MinimumLength =3)]
        [Display(Name = "Movie Name")]
        public string? Title { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Release Date")]
        public DateTime? ReleaseDate { get; set; }

        [Required]
        [Display(Name = "Genre")]
        public int GenreId { get; set; }

        [Required]
        [Range(0, 999.99)]
        [Display(Name = "Price")]
        public decimal? Price { get; set; }
    }
}
