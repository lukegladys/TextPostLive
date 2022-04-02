using Microsoft.AspNetCore.SignalR;
using TextPostLive.Data.Repository;
using TextPostLive.Data.Service;

namespace TextPostLive.UI;

public class TextPostLiveHub : Hub
{
    public const string HubUrl = "/post";
    private readonly TextPostServiceClient _textPostService;

    public TextPostLiveHub(TextPostServiceClient textPostService)
    {
        _textPostService = textPostService;
    }

    public async Task Broadcast(string message)
    {
        var newTextPost = await _textPostService.SaveTextPost(message);
        await Clients.All.SendAsync("Broadcast", newTextPost);
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
