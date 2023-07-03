using BookComicsWebApi.DTOs.RequestModels;
using BookComicsWebApi.DTOs.ResponseModels;

namespace BookComicsWebApi.Contracts
{
    public interface IBookRepository
    {
        Task<IEnumerable<BookResponseModel>> GetTopRatedBooksAsync(bool isChecked);
        Task<IEnumerable<BookResponseModel>> GetSearchedBooks(bool isChecked, string searchTerm);
        Task<HttpResponseMessage> RateBook(int rating, int bookId);
        Task<IEnumerable<BookResponseModel>> GetPaginatedData(bool isChecked, int skip, int take);
        Task<HttpResponseMessage> UploadBooks(List<BookDTO> books);

    }
}
