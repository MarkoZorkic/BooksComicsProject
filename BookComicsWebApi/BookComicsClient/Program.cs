using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
//builder.Services.Configure<WebApiOptions>(builder.Configuration.GetSection("WebApi"));
//builder.Services.AddHttpClient("my app", client =>
//{
//    client.BaseAddress = new Uri(builder.Configuration.GetSection("WebApi:Url").Value);
//});
builder.Services.AddHttpClient();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Book}/{action=GetTableData}/{id?}");
});

app.MapRazorPages();

app.Run();
