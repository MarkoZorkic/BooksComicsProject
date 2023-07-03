
using BooksComics.DTOs.ResponseModels;
using BooksComics.Helpers;
using BooksComics.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Text;

namespace BooksComics.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppSettings _appSettings;
        private readonly AuthorizationKey _apiKey;
        public HomeController(IOptions<AppSettings> appSettings, IOptions<AuthorizationKey> apiKey)
        {
            _appSettings = appSettings.Value;
            _apiKey = apiKey.Value;
        }
        private readonly int PageSize = 5;
        //public async Task<IActionResult> Index(bool isChecked)
        //{
        //    var topRatedBooks = await _bookRepository.GetTopRatedBooksAsync(isChecked);
        //    return View(topRatedBooks);
        //}

        public async Task<IActionResult> GetTableData(bool isChecked = true)
        {
            string baseUrl = _appSettings.BaseUrl;
            string apiKey = _apiKey.ApiKey;
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("ApiKey", apiKey);
            IEnumerable<BookResponseModel> books = null!;
            client.BaseAddress = new Uri(baseUrl);

            var response = await client.GetAsync("api/Book/GetTableData");

            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync().Result;
                books = JsonConvert.DeserializeObject<List<BookResponseModel>>(result)!;
            }
            return View("_TableDataPartial",books);
        }
        public async Task<IActionResult> GetTableDataPartial(bool isChecked)
        {
            string baseUrl = _appSettings.BaseUrl;
            string apiKey = _apiKey.ApiKey;

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("ApiKey", apiKey);

            IEnumerable<BookResponseModel> topRatedBooks = null!;
            client.BaseAddress = new Uri(baseUrl);

            var response = await client.GetAsync("api/Book/GetTableDataPartial?isChecked=" + isChecked.ToString());

            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync().Result;
                topRatedBooks = JsonConvert.DeserializeObject<List<BookResponseModel>>(result)!;
            }
            return PartialView("Results", topRatedBooks ?? new List<BookResponseModel>());
        }
        public async Task<IActionResult> IndexWithSearchTerm(bool isChecked, string searchTerm)
        {
            string baseUrl = _appSettings.BaseUrl;
            string apiKey = _apiKey.ApiKey;

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("ApiKey", apiKey);

            IEnumerable<BookResponseModel> searchedData = null!;
            client.BaseAddress = new Uri(baseUrl);

            var response = await client.GetAsync("api/Book/IndexWithSearchTerm?isChecked=" + isChecked.ToString().ToLower() + "&searchTerm=" + searchTerm);
            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync().Result;
                searchedData = JsonConvert.DeserializeObject<List<BookResponseModel>>(result)!;
            }
            return PartialView("Results",searchedData ?? new List<BookResponseModel>());
        }
        public async Task<IActionResult> GetAdditionalContent(bool isChecked, int pageNumber)
        {
            string baseUrl = _appSettings.BaseUrl;
            string apiKey = _apiKey.ApiKey;

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("ApiKey", apiKey);

            IEnumerable<BookResponseModel> additionalContent = null!;
            client.BaseAddress = new Uri(baseUrl);

            // Get the additional content
            var response = await client.GetAsync($"api/Book/GetAdditionalContent?isChecked={isChecked}&pageNumber={pageNumber}");
            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync().Result;
                additionalContent = JsonConvert.DeserializeObject<List<BookResponseModel>>(result)!;
            }

            return PartialView("_AdditionalContent", additionalContent);
        }

        public async Task<HttpResponseMessage> RateBook(int rating, int bookId)
        {
            string baseUrl = _appSettings.BaseUrl;
            string apiKey = _apiKey.ApiKey;

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(baseUrl);
            client.DefaultRequestHeaders.Add("ApiKey", apiKey);
            StringContent httpContent = new StringContent(JsonConvert.SerializeObject(new { Rating=rating, BookId=bookId }), System.Text.Encoding.UTF8, "application/json");

            var response = await client.PostAsync($"api/Book/RateBook", httpContent);
            return new HttpResponseMessage(System.Net.HttpStatusCode.OK);

        }

        public IActionResult Privacy()
        {
            return View();
        }

        //private async Task<IEnumerable<BookResponseModel>> GetItems(bool isChecked, int offset, int count)
        //{
        //    return await _bookRepository.GetPaginatedData(isChecked, offset, count);
        //}
    }
    
}