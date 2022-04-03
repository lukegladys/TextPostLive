using Microsoft.AspNetCore.ResponseCompression;
using TextPostLive.UI;
using TextPostLive.Data.Repository;
using StackExchange.Redis;
using TextPostLive.Data.Model;
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
var multiplexer = ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("RedisCache"));
builder.Services.AddSingleton<IConnectionMultiplexer>(multiplexer);

// Sql
builder.Services.AddSqlServer<TextPostDbContext>(builder.Configuration.GetConnectionString("SqlServer"));

builder.Services.AddScoped<RedisTextPostRepository>();
builder.Services.AddScoped<SqlTextPostRepository>();
builder.Services.AddScoped<TextPostService>();

// SignalR
if (!builder.Environment.IsDevelopment())
{
    builder.Services.AddSignalR().AddAzureSignalR(options =>
    {
        options.ConnectionString = builder.Configuration.GetConnectionString("AzureSignalR");
        options.ServerStickyMode =
            Microsoft.Azure.SignalR.ServerStickyMode.Required;
    });
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/api/posts", async (TextPostService service) =>
    await service.GetTextPostsAsync());

app.MapPost("/api/newpost", async (TextPost post, TextPostService service) =>
{
    var textPost = await service.SaveTextPostAsync(post);
    return Results.Created($"/api/posts", post);
})
.WithName("PostNewTextPost");

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapHub<TextPostLiveHub>(TextPostLiveHub.HubUrl);

app.MapFallbackToPage("/_Host");

app.Run();
