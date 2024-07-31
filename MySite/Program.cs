using MySite.AutoMapper;
using MySite.Components;
using MySite.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Configuration.AddJsonFile("appsettings.json")
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true);

builder.Services.AddScoped<ITweetService, TweetService>();
builder.Services.AddScoped<IBlogService, BlogService>();

var apiEndpoint = builder.Configuration.GetValue<string>("APIEndpoint") ?? throw new Exception("APIEndpoint empty in appsettings");

builder.Services.AddSingleton(x => new HttpClient
{
    BaseAddress = new Uri(apiEndpoint)
});

builder.Services.AddAutoMapper(config =>
{
    config.AddProfile<LikedTweetProfile>();
    config.AddProfile<BlogProfile>();
});

builder.Services.AddHttpLogging(config =>
{
    config.CombineLogs = true;
});

var app = builder.Build();

app.UseHttpLogging();

app.UseStaticFiles();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
