using System;
using System.Collections.Generic;

namespace MovieApplication.Models
{
    public partial class Movie
    {
        public int MovieId { get; set; }
        public string? Title { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public int GenreId { get; set; }
        public decimal? Price { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual Genre Genre { get; set; } = null!;
    }
}
