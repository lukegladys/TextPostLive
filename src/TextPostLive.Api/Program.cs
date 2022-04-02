using StackExchange.Redis;
using TextPostLive.Data.Model;
using TextPostLive.Data.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Redis
var multiplexer = ConnectionMultiplexer.Connect(builder.Configuration["CacheConnection"]);
builder.Services.AddSingleton<IConnectionMultiplexer>(multiplexer);
builder.Services.AddStackExchangeRedisCache(option =>
{
    option.Configuration = builder.Configuration["CacheConnection"];
});

builder.Services.AddSingleton<RedisTextPostRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/api/posts", async (RedisTextPostRepository repository) =>
    await repository.GetTextPostsAsync());

app.MapPost("/api/newpost", async (TextPost post, RedisTextPostRepository repository) =>
{
    var textPost = await repository.SaveTextPostAsync(post);
    return Results.Created($"/api/posts", post);
})
.WithName("PostNewTextPost");

app.Run();