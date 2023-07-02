using BookComicsClient.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BookComicsClient.Controllers
{
    public class BookController : Controller
    {
        private readonly int PageSize = 5;

        public const string BASE_ADDRESS = "http://localhost:7218/";
        //private readonly HttpClient _httpClient;
        public BookController(HttpClient httpClient)
        {
            //_httpClient = httpClient;
        }
        //public IActionResult Index()
        //{
        //    return View();
        //}

        public async Task<IActionResult> GetTableData()
        {
            HttpClient client = new HttpClient();

            IEnumerable<BookWebApiModel> books = null!;
            client.BaseAddress = new Uri(BASE_ADDRESS);
            var response = await client.GetAsync("api/Book/GetTableData");
         
            if (response.IsSuccessStatusCode) 
            { 
                var result = response.Content.ReadAsStringAsync().Result;
                books = JsonConvert.DeserializeObject<List<BookWebApiModel>>(result)!;
            }

            return View("GetTableData", books);
        }

        public async Task<IActionResult> GetTableDataPartial(bool isChecked)
        {
            HttpClient client = new HttpClient();
            IEnumerable<BookWebApiModel> topRatedBooks = null!;
            var response = await client.GetAsync($"api/Book/GetTopRatedBooksAsync?isChecked={isChecked}");

            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync().Result;
                topRatedBooks = JsonConvert.DeserializeObject<List<BookWebApiModel>>(result)!;
            }

            return Ok(topRatedBooks);
        }

        public async Task<IActionResult> GetAdditionalContent(bool isChecked, int pageNumber)
        {
            HttpClient client = new HttpClient();
            IEnumerable<BookWebApiModel> additionalContent = null!;

            // Get the additional content
            var response = await client.GetAsync($"api/Book/GetAdditionalContent?isChecked={isChecked}&pageNumber={pageNumber}");
            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync().Result;
                additionalContent = JsonConvert.DeserializeObject<List<BookWebApiModel>>(result)!;
            }

            // Return the additional content as a partial view
            return Ok( additionalContent);
        }

        //public async Task<IActionResult> RateBook(int rating, int bookId)
        //{

        //}
    }
}
