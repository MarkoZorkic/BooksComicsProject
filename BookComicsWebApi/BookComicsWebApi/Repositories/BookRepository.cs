using BookComicsWebApi.Contracts;
using BookComicsWebApi.Data.Models;
using BookComicsWebApi.Data;
using BookComicsWebApi.DTOs.ResponseModels;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using BookComicsWebApi.DTOs.RequestModels;

namespace BookComicsWebApi.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly BooksComicDbContext _context;
        public BookRepository(BooksComicDbContext context)
        {
            _context = context;

        }

        public async Task<IEnumerable<BookResponseModel>> GetSearchedBooks(bool isChecked, string searchTerm)
        {
            var query = _context.Books.Where(x => x.IsBook == isChecked).ToList();
            List<Book> books = new List<Book>();
            if (!string.IsNullOrEmpty(searchTerm))
            {
                var afterYearPattern = new Regex(@"^after\s*(\d{4})$", RegexOptions.IgnoreCase);
                var starPattern = new Regex(@"^(\d+)\s*star$", RegexOptions.IgnoreCase);
                var olderThanYearsPattern = new Regex(@"^older\s*than\s*(\d+)\s*years$", RegexOptions.IgnoreCase);

                var afterYearMatch = afterYearPattern.Match(searchTerm);
                var starMatch = starPattern.Match(searchTerm);
                var olderThanYearsMatch = olderThanYearsPattern.Match(searchTerm);

                if (afterYearMatch.Success)
                {
                    var year = int.Parse(afterYearMatch.Groups[1].Value);

                    books = query.Where(x => x.ReleaseDate.Year > year).ToList();
                }
                else if (starMatch.Success)
                {
                    var starRating = int.Parse(starMatch.Groups[1].Value);

                    books = query.Where(x => GetAverageRate(x.Id) >= starRating).ToList();
                }
                else if (olderThanYearsMatch.Success)
                {
                    var years = int.Parse(olderThanYearsMatch.Groups[1].Value);
                    var cutoffDate = DateTime.Today.AddYears(-years);
                    books = query.Where(x => x.ReleaseDate < cutoffDate).ToList();
                }

                else
                {
                    books = query.Where(x => x.Title.Contains(searchTerm)).ToList();

                }

            }

            return books.Select(b => new BookResponseModel
            {
                Id = b.Id,
                Title = b.Title,
                Description = b.Description,
                AverageRate = GetAverageRate(b.Id),
                ReleaseDate = b.ReleaseDate
            });
        }

        public async Task<IEnumerable<BookResponseModel>> GetTopRatedBooksAsync(bool isChecked)
        {
            var topRatedBooks = await _context.Books.Where(x => x.IsBook == isChecked).ToListAsync();

            return topRatedBooks.Select(x => new BookResponseModel
            {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description,
                ReleaseDate = x.ReleaseDate,
                ImagePath = x.ImagePath,
                AverageRate = GetAverageRate(x.Id)
            })
                .OrderByDescending(x => x.AverageRate)
                .Take(5);
        }

        public async Task<HttpResponseMessage> RateBook(int rating, int bookId)
        {
            _context.BookRates.Add(new BookRate
            {
                BookId = bookId,
                Rate = rating
            });
            _context.SaveChanges();

            return new HttpResponseMessage(System.Net.HttpStatusCode.OK);
        }

        public async Task<IEnumerable<BookResponseModel>> GetPaginatedData(bool isChecked, int skip, int take)
        {
            var result = await _context.Books
                .Where(x => x.IsBook == isChecked)
                .OrderByDescending(x => _context.BookRates
                    .Where(r => r.BookId == x.Id)
                    .Average(r => r.Rate))
                .Skip(skip)
                .Take(take)
                .Select(x => new BookResponseModel
                {
                    Id = x.Id,
                    Title = x.Title,
                    Description = x.Description,
                    ReleaseDate = x.ReleaseDate,
                    ImagePath = x.ImagePath,
                    AverageRate = (decimal?)_context.BookRates
                        .Where(r => r.BookId == x.Id)
                        .Average(r => r.Rate)
                })
                .ToListAsync();

            return result.AsEnumerable();
        }

        private decimal GetAverageRate(int id)
        {
            decimal sumRates = 0;
            var items = _context.BookRates.Where(x => x.BookId == id).ToList();
            var ratesCount = items.Count();
            foreach (var item in items)
            {
                sumRates += item.Rate;
            }
            var result = sumRates / ratesCount;
            return Math.Round((sumRates / ratesCount), 2);

        }

        public async Task<HttpResponseMessage> UploadBooks(List<BookDTO> books)
        {
            try
            {
                var newBooks = new List<Book>();
                var newActors = new List<Actor>();
                var newBookRates = new List<BookRate>();
               
                foreach (var book in books)
                {
                   
                    var newBook = new Book
                    {
                        Title = book.Title,
                        Description = book.Description,
                        ReleaseDate = book.ReleaseDate,
                        ImagePath = book.ImagePathBase64,
                        IsBook = book.IsBook
                    };

                    newBooks.Add(newBook);

                    foreach (var actorDto in book.Actors)
                    {
                        var newActor = new Actor
                        {
                            Name = actorDto.Name,
                            Book = newBook
                        };

                        newActors.Add(newActor);
                    }

                    foreach (var bookRateDto in book.BookRates)
                    {
                        var newBookRate = new BookRate
                        {
                            Book = newBook,
                            Rate = bookRateDto.Rate
                        };

                        newBookRates.Add(newBookRate);
                    }
                }

                _context.Books.AddRange(newBooks);
                _context.Actors.AddRange(newActors);
                _context.BookRates.AddRange(newBookRates);

                // Save changes to the database
                _context.SaveChanges();

                return new HttpResponseMessage(System.Net.HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return new HttpResponseMessage(System.Net.HttpStatusCode.NotFound);
            }
        }

        private string ConvertImageToBase64(string imagePath)
        {
            byte[] imageBytes = System.IO.File.ReadAllBytes(imagePath);
            string base64String = Convert.ToBase64String(imageBytes);
            return base64String;
        }
    }
}
