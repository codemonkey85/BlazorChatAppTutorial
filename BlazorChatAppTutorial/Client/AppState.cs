using BlazorChatAppTutorial.Client.Pages;
using BlazorChatAppTutorial.Shared.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
                if (CurrentRoom != null && string.Equals(CurrentRoom.RoomName, roomName))
                {
                    CurrentRoom.ReceiveMessage(roomName, chatMessage);
                }
                else
                {
                    if (JoinedRooms.TryGetValue(roomName, out ChatRoomModel joinedRoom))
                    {
                        joinedRoom.UnreadCount++;
                        AppStateUpdated?.Invoke();
                    }
                }
            });

            HubConnection.StartAsync();
        }

        private RoomComponent CurrentRoom { get; set; }

        public async Task SetupHubConnection(ChatRoomModel chatRoom, RoomComponent roomComponent = null)
        {
            if (roomComponent != null)
            {
                CurrentRoom = roomComponent;
            }

            if (!JoinedRooms.ContainsKey(chatRoom.RoomName))
            {
                JoinedRooms.Add(chatRoom.RoomName, chatRoom);
            }
            await HubConnection.SendAsync("JoinRoom", chatRoom.RoomName);
            AppStateUpdated?.Invoke();
        }

        public async Task SendAsync(string roomName, ChatMessageModel chatMessage)
        {
            await HubConnection.SendAsync("SendMessage", roomName, chatMessage);
        }

        public bool IsHubConnected => HubConnection.State == HubConnectionState.Connected;

        [Required]
        public string UserName { get; set; }

        public IDictionary<string, ChatRoomModel> JoinedRooms { get; } = new Dictionary<string, ChatRoomModel>();

        public Action AppStateUpdated { get; set; }
    }
}
