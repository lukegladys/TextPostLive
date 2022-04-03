using Microsoft.AspNetCore.ResponseCompression;
using TextPostLive.UI;
using TextPostLive.Data.Repository;
using StackExchange.Redis;
using TextPostLive.Data.Service;

var builder = WebApplication.CreateBuilder(args);

// Add app services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddResponseCompression(opts =>
{
    opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
        new[] { "application/octet-stream" });
});

// Redis
var multiplexer = ConnectionMultiplexer.Connect(builder.Configuration["CacheConnection"]);
builder.Services.AddSingleton<IConnectionMultiplexer>(multiplexer);

// Sql
builder.Services.AddSqlServer<TextPostDbContext>(builder.Configuration.GetConnectionString("AzureSql"));

builder.Services.AddScoped<RedisTextPostRepository>();
builder.Services.AddScoped<SqlTextPostRepository>();
builder.Services.AddScoped<TextPostService>();

// SignalR
builder.Services.AddSignalR();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/api/posts", async (TextPostService service) =>
    await service.GetTextPostsAsync());

app.MapPost("/api/newpost", async (string post, TextPostService service) =>
{
    var textPost = await service.CacheTextPostAsync(post);
    await service.SaveTextPostAsync(textPost);
    return Results.Created($"/api/posts", textPost);
})
.WithName("PostNewTextPost");

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");
app.MapHub<TextPostLiveHub>(TextPostLiveHub.HubUrl);

app.Run();
