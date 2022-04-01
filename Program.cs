using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.ResponseCompression;
using StackExchange.Redis;
using TextPostLive;
using TextPostLive.Data;

var builder = WebApplication.CreateBuilder(args);

// Add app services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<TextPostService>();
builder.Services.AddResponseCompression(opts =>
{
    opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
        new[] { "application/octet-stream" });
});

// Redis
var multiplexer = ConnectionMultiplexer.Connect(builder.Configuration["CacheConnection"]);
builder.Services.AddSingleton<IConnectionMultiplexer>(multiplexer);

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

app.MapBlazorHub();
app.MapHub<TextPostLiveHub>(TextPostLiveHub.HubUrl);

app.MapFallbackToPage("/_Host");

app.Run();
