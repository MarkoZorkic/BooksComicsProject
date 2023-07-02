namespace BookComicsWebApi.Middleware
{
    public class ApiKeyMiddleware
    {
        private readonly RequestDelegate _next;

        public ApiKeyMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            
            string apiKey = context.Request.Headers["ApiKey"];
            string validApiKey = "your-valid-api-key"; // Replace with your actual valid API key
            if (string.IsNullOrEmpty(apiKey))
            {
                // Check if the API key is passed as a query parameter
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
