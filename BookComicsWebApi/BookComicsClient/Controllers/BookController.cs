using BookComicsClient.Helpers;
using BookComicsClient.Models;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BookComicsClient.Controllers
{
    public class BookController : Controller
    {

        private readonly AppSettings _appSettings;
        private readonly AuthorizationKey _apiKey;


        public BookController(IOptions<AppSettings> appSettings, IOptions<AuthorizationKey> apiKey)
        {
            _appSettings = appSettings.Value;
            _apiKey = apiKey.Value;
        }
       
        public async Task<IActionResult> GetTableData()
        {
            string baseUrl = _appSettings.BaseUrl;
            string apiKey = _apiKey.ApiKey;
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("ApiKey", apiKey);
            IEnumerable<BookWebApiModel> books = null!;
            client.BaseAddress = new Uri(baseUrl);
            
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
            string baseUrl = _appSettings.BaseUrl;
            string apiKey = _apiKey.ApiKey;

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("ApiKey", apiKey);

            IEnumerable<BookWebApiModel> topRatedBooks = null!;
            client.BaseAddress = new Uri(baseUrl);

            var response = await client.GetAsync("api/Book/GetTableDataPartial?isChecked="+isChecked.ToString());

            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync().Result;
                topRatedBooks = JsonConvert.DeserializeObject<List<BookWebApiModel>>(result)!;
            }

            return PartialView("Results",topRatedBooks);
        }

        public async Task<IActionResult> IndexWithSearchTerm(bool isChecked, string searchTerm)
        {
            string baseUrl = _appSettings.BaseUrl;
            string apiKey = _apiKey.ApiKey;

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("ApiKey", apiKey);

            IEnumerable<BookWebApiModel> searchedData = null!;
            client.BaseAddress = new Uri(baseUrl);

            var response = await client.GetAsync("api/Book/IndexWithSearchTerm?isChecked="+isChecked.ToString().ToLower()+"&searchTerm="+searchTerm);
            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync().Result;
                searchedData = JsonConvert.DeserializeObject<List<BookWebApiModel>>(result)!;
            }

            return PartialView("Results",searchedData);
        }

        public async Task<IActionResult> GetAdditionalContent(bool isChecked, int pageNumber)
        {
            string baseUrl = _appSettings.BaseUrl;
            string apiKey = _apiKey.ApiKey;

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("ApiKey", apiKey);

            IEnumerable<BookWebApiModel> additionalContent = null!;
            client.BaseAddress = new Uri(baseUrl);

            var response = await client.GetAsync($"api/Book/GetAdditionalContent?isChecked={isChecked}&pageNumber={pageNumber}");
            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync().Result;
                additionalContent = JsonConvert.DeserializeObject<List<BookWebApiModel>>(result)!;
            }

            return Ok( additionalContent);
        }

        //public async Task<IActionResult> RateBook(int rating, int bookId)
        //{

        //}
    }
}
