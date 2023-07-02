using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BookComicsWebApi.Data.Models
{
    public class Book
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Title { get; set; } = default!;
        public string Description { get; set; } = default!;
        public DateTime ReleaseDate { get; set; }
        public string ImagePath { get; set; } = default!;
        public bool IsBook { get; set; }
        public List<Actor> Actors { get; set; } = default!;
        public List<BookRate> BookRates { get; set; } = default!;
    }
}
