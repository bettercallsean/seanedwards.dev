using Microsoft.Extensions.FileProviders;
using MySite.AutoMapper;
using MySite.Components;
using MySite.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Configuration.AddJsonFile("appsettings.json")
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true);

builder.Services.AddScoped<ITweetService, TweetService>();
builder.Services.AddSingleton(x => new HttpClient
{
    BaseAddress = new Uri(builder.Configuration["APIEndpoint"])
});

builder.Services.AddAutoMapper(config =>
{
    config.AddProfile<LikedTweetProfile>();
});

var app = builder.Build();

app.UseStaticFiles();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
