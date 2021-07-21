using System.Collections.Generic;
using System.Threading.Tasks;
using BlazorChatAppTutorial.Server.Data;
using BlazorChatAppTutorial.Shared.Models;
using Microsoft.AspNetCore.SignalR;

namespace BlazorChatAppTutorial.Server.Hubs
{
    public class ChatHub : Hub
    {
        public PreviousChatArchive PreviousChatArchive { get; }

        public ChatHub(PreviousChatArchive previousChatArchive)
        {
            PreviousChatArchive = previousChatArchive;
        }

        public async Task JoinRoom(string roomName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, roomName);
        }

        public async Task SendMessage(string roomName, ChatModel chatModel)
        {
            if (!PreviousChatArchive.Chats.ContainsKey(roomName))
            {
                PreviousChatArchive.Chats.Add(roomName, new List<ChatModel>());
            }
            PreviousChatArchive.Chats[roomName].Add(chatModel);
            await Clients.Group(roomName).SendAsync("ReceiveMessage", chatModel);
        }
    }
}
