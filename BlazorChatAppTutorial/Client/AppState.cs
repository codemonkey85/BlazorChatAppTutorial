using BlazorChatAppTutorial.Client.Pages;
using BlazorChatAppTutorial.Shared.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorChatAppTutorial.Client
{
    public class AppState
    {
        private IDictionary<string, HubConnection> RoomHubConnections { get; set; } = new Dictionary<string, HubConnection>();

        private NavigationManager NavigationManager { get; set; }

        public AppState(NavigationManager navigationManager)
        {
            NavigationManager = navigationManager;
        }

        public Room CurrentRoom { get; set; }

        public async Task SetupHubConnection(string roomName)
        {
            if (RoomHubConnections.TryGetValue(roomName, out HubConnection hubConnection))
            {
                return;
            }

            hubConnection = new HubConnectionBuilder()
                .WithUrl(NavigationManager.ToAbsoluteUri("/chathub"))
                .Build();

            hubConnection.On<ChatMessageModel>("ReceiveMessage", chatMessage =>
            {
                CurrentRoom.ReceiveMessage(chatMessage);
            });

            await hubConnection.StartAsync();
            await hubConnection.SendAsync("JoinRoom", roomName);

            RoomHubConnections.Add(roomName, hubConnection);

            return;
        }

        public async Task SendAsync(string roomName, ChatMessageModel chatMessage)
        {
            if (RoomHubConnections.TryGetValue(roomName, out HubConnection hubConnection))
            {
                await hubConnection.SendAsync("SendMessage", roomName, chatMessage);
            }
        }

        public bool IsRoomHubConnected(string roomName) =>
            RoomHubConnections.TryGetValue(roomName, out HubConnection hubConnection) &&
            hubConnection.State == HubConnectionState.Connected;

        public string UserName { get; set; }

        public List<string> RoomNames { get; set; } = new List<string>();

        public Action AppStateUpdated { get; set; }
    }
}
