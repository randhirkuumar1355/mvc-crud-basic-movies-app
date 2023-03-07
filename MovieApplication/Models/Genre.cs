using System;
using System.Collections.Generic;

namespace MovieApplication.Models
{
    public partial class Genre
    {
        public Genre()
        {
            Movies = new HashSet<Movie>();
        }

        public int GenreId { get; set; }
        public string? Title { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual ICollection<Movie> Movies { get; set; }
    }
}
