namespace BooksComics.DTOs.RequestModels
{
    public class BookDTO
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string ImagePathBase64 { get; set; }
        public bool IsBook { get; set; }
        public List<ActorDTO> Actors { get; set; }
        public List<BookRateDTO> BookRates { get; set; }
    }

    public class ActorDTO
    {
        public string Name { get; set; }
    }

    public class BookRateDTO
    {
        public int BookId { get; set; }
        public int Rate { get; set; }
    }
}
