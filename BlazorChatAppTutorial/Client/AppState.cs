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
        public HubConnection HubConnection { get; set; }

        public AppState(NavigationManager navigationManager)
        {
            if (navigationManager == null)
            {
                return;
            }

            HubConnection = new HubConnectionBuilder()
                .WithUrl(navigationManager.ToAbsoluteUri("/chathub"))
                .Build();

            HubConnection.On<string, ChatMessageModel>("ReceiveMessage", (roomName, chatMessage) =>
            {
                if (string.Equals(CurrentRoom.RoomName, roomName))
                {
                    CurrentRoom.ReceiveMessage(roomName, chatMessage);
                }
                else
                {
                    // notification
                }
            });

            HubConnection.StartAsync();
        }

        private Room CurrentRoom { get; set; }

        public async Task SetupHubConnection(Room room)
        {
            CurrentRoom = room;

            if (Rooms.TryGetValue(CurrentRoom.RoomName, out bool isConfigured) && isConfigured)
            {
                return;
            }
            if (!Rooms.ContainsKey(CurrentRoom.RoomName))
            {
                Rooms.Add(CurrentRoom.RoomName, false);
            }

            await HubConnection.SendAsync("JoinRoom", CurrentRoom.RoomName);
            Rooms[CurrentRoom.RoomName] = true;
            AppStateUpdated?.Invoke();
        }

        public async Task SendAsync(string roomName, ChatMessageModel chatMessage)
        {
            await HubConnection.SendAsync("SendMessage", roomName, chatMessage);
        }

        public bool IsHubConnected => HubConnection.State == HubConnectionState.Connected;

        public bool TryAddRoom(string roomName)
        {
            if (!Rooms.ContainsKey(roomName))
            {
                Rooms.Add(roomName, false);
                return true;
            }
            return false;
        }

        public string UserName { get; set; }

        private IDictionary<string, bool> Rooms { get; set; } = new Dictionary<string, bool>();

        public ICollection<string> RoomNames => Rooms.Keys;

        public Action AppStateUpdated { get; set; }
    }
}
