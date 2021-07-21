using BlazorChatAppTutorial.Server.Data;
using BlazorChatAppTutorial.Shared.Models;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorChatAppTutorial.Server.Hubs
{
    public class ChatHub : Hub
    {
        public PreviousChatArchive PreviousChatArchive { get; }

        public ChatHub(PreviousChatArchive previousChatArchive)
        {
            PreviousChatArchive = previousChatArchive;
        }

        public async Task SendMessage(string roomName, string userName, string message)
        {
            if (!PreviousChatArchive.Chats.ContainsKey(roomName))
            {
                PreviousChatArchive.Chats.Add(roomName, new List<ChatModel>());
            }
            PreviousChatArchive.Chats[roomName].Add(new ChatModel { UserName = userName, Message = message });
            await Clients.All.SendAsync("ReceiveMessage", userName, message);
        }
    }
}
