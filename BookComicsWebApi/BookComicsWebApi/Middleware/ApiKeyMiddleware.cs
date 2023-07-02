using BookComicsWebApi.Helpers;
using Microsoft.Extensions.Options;

namespace BookComicsWebApi.Middleware
{
    public class ApiKeyMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly AuthorizationKey _apiKey;

        public ApiKeyMiddleware(RequestDelegate next, IOptions<AuthorizationKey> apiKey)
        {
            _next = next;
            _apiKey = apiKey.Value;
        }

        public async Task Invoke(HttpContext context)
        {
            
            string apiKey = context.Request.Headers["ApiKey"];
            string validApiKey = _apiKey.ApiKey; 
            
            if (string.IsNullOrEmpty(apiKey))
            {
                apiKey = context.Request.Headers["Authorization"]; ;
            }
            if (apiKey != validApiKey)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return;
            }

            await _next.Invoke(context);
        }
    }
}
