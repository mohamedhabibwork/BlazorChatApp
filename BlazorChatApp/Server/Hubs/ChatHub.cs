using Microsoft.AspNetCore.SignalR;

namespace BlazorChatApp.Server.Hubs
{
    public class ChatHub:Hub
    {
        private static IDictionary<string,string> _users = new Dictionary<string,string>();
        public override async  Task OnConnectedAsync()
        {
            var username = Context.GetHttpContext().Request.Query["username"];
            _users.Add(Context.ConnectionId,username);
            await AddMessageToChat("", $"{username} joined to party ☺");
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var user = _users.FirstOrDefault(u => u.Key.Equals(Context.ConnectionId));
            _users.Remove(user);
            await AddMessageToChat("", $"{user.Value} leaved  party ↓");
            await base.OnDisconnectedAsync(exception);
        }

        public async Task AddMessageToChat(string user,string message)
        {
            Console.WriteLine(message);
            await Clients.All.SendAsync("GetThatMessageDude",user, message);
        }

    }
}
