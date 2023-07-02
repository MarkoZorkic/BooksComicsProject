using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BookComicsWebApi.Data.Models
{
    public class BookRate
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int Rate { get; set; }
        public Book Book { get; set; } = default!;
        public int BookId { get; set; }
    }
}
