using Microsoft.AspNetCore.SignalR;
using TextPostLive.Data.Repository;
using TextPostLive.Data.Service;

namespace TextPostLive.UI;

public class TextPostLiveHub : Hub
{
    public const string HubUrl = "/post";
    private readonly TextPostService _textPostService;

    public TextPostLiveHub(TextPostService textPostService)
    {
        _textPostService = textPostService;
    }

    public async Task Broadcast(string message)
    {
        var newTextPost = await _textPostService.CacheTextPostAsync(message);
        await Clients.All.SendAsync("Broadcast", message);
        await _textPostService.SaveTextPostAsync(newTextPost);
    }

    public override Task OnConnectedAsync()
    {
        Console.WriteLine($"{Context.ConnectionId} connected");
        return base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? e)
    {
        Console.WriteLine($"Disconnected {e?.Message} {Context.ConnectionId}");
        await base.OnDisconnectedAsync(e);
    }
}
