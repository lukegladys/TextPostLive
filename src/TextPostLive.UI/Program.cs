using Microsoft.AspNetCore.ResponseCompression;
using TextPostLive.UI;
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

builder.Services.AddHttpClient<TextPostServiceClient>(httpClient =>
{
    httpClient.BaseAddress = new Uri(builder.Configuration["TextPostLiveApiBaseAddress"]);
});

if (!builder.Environment.IsDevelopment())
{
    builder.Services.AddSignalR().AddAzureSignalR();
}

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
