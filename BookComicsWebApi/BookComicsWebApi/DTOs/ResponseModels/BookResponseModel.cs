namespace BookComicsWebApi.DTOs.ResponseModels
{
    public class BookResponseModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = default!;
        public string Description { get; set; } = default!;
        public DateTime ReleaseDate { get; set; }
        public string ImagePath { get; set; } = default!;
        public decimal? AverageRate { get; set; }
    }
}
