using BookComicsWebApi.Contracts;
using BookComicsWebApi.DTOs.RequestModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookComicsWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookRepository _bookRepository;
        public BookController(IBookRepository bookRepository)
        {
                _bookRepository = bookRepository;
        }

        private readonly int PageSize = 5;

        [HttpGet("GetTableData")]
        public async Task<IActionResult> GetTableData(bool isChecked = true)
        {
            var topRatedBooks = await _bookRepository.GetTopRatedBooksAsync(isChecked);
            return Ok(topRatedBooks);
        }

        [HttpGet("GetTableDataPartial")]
        public async Task<IActionResult> GetTableDataPartial(bool isChecked)
        {
            var topRatedBooks = await _bookRepository.GetTopRatedBooksAsync(isChecked);
            return Ok(topRatedBooks);
        }

        [HttpGet("IndexWithSearchTerm")]
        public async Task<IActionResult> IndexWithSearchTerm(bool isChecked, string searchTerm)
        {
            var searchedBooks = await _bookRepository.GetSearchedBooks(isChecked, searchTerm);
            return Ok(searchedBooks);
        }

        [HttpGet("GetAdditionalContent")]

        public async Task<IActionResult> GetAdditionalContent(bool isChecked, int pageNumber)
        {
            int offset = pageNumber * PageSize;
            var additionalContent = await _bookRepository.GetPaginatedData(isChecked, offset, PageSize);
            return Ok(additionalContent);
        }

        [HttpPost("RateBook")]
        public async Task<HttpResponseMessage> RateBook(RateModel model)
        {
            return await _bookRepository.RateBook(model.Rating, model.BookId);
        }

        [HttpPost("UploadBooks")]
        public async Task<HttpResponseMessage> UploadBooks(List<BookDTO> books)
        {
            return await _bookRepository.UploadBooks(books);
        }

    }

    public class RateModel
    {
        public int Rating { get; set; }
        public int BookId { get; set; }

    }
}
