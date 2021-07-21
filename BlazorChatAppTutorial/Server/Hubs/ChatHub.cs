using BlazorChatAppTutorial.Server.Data;
using BlazorChatAppTutorial.Shared.Models;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorChatAppTutorial.Server.Hubs
{
    public class ChatHub : Hub
    {
        public PreviousChatArchive PreviousChatArchive { get; }

        public ChatHub(PreviousChatArchive previousChatArchive) => PreviousChatArchive = previousChatArchive;

        public async Task JoinRoom(string roomName) => await Groups.AddToGroupAsync(Context.ConnectionId, roomName);

        public async Task SendMessage(string roomName, ChatMessageModel chatMessage)
        {
            if (!PreviousChatArchive.Chats.ContainsKey(roomName))
            {
                PreviousChatArchive.Chats.Add(roomName, new List<ChatMessageModel>());
            }
            chatMessage.DateSent = DateTime.Now;
            PreviousChatArchive.Chats[roomName].Add(chatMessage);
            await Clients.Group(roomName).SendAsync("ReceiveMessage", roomName, chatMessage);
        }
    }
}
