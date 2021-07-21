using BlazorChatAppTutorial.Shared.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace BlazorChatAppTutorial.Client.Pages
{
    [Route("/room/{RoomName}")]
    public partial class Room : IAsyncDisposable
    {
        [Inject] private NavigationManager NavigationManager { get; set; }

        [Inject] private HttpClient HttpClient { get; set; }

        [Parameter] public string RoomName { get; set; }

        private HubConnection hubConnection;
        private readonly List<ChatMessageModel> ChatMessages = new();
        private readonly ChatMessageModel newChatMessage = new();

        protected override async Task OnParametersSetAsync()
        {
            newChatMessage.UserName = AppState.UserName;

            ChatMessages.Clear();

            if (hubConnection != null)
            {
                await hubConnection.DisposeAsync();
            }

            List<ChatMessageModel> previousChatMessages = (await HttpClient.GetFromJsonAsync<IEnumerable<ChatMessageModel>>($"chat/{RoomName}")).ToList();
            if (previousChatMessages?.Any() ?? false)
            {
                ChatMessages.AddRange(previousChatMessages);
                StateHasChanged();
            }

            hubConnection = new HubConnectionBuilder()
                .WithUrl(NavigationManager.ToAbsoluteUri("/chathub"))
                .Build();

            hubConnection.On<ChatMessageModel>("ReceiveMessage", chatMessage =>
            {
                ChatMessages.Add(chatMessage);
                StateHasChanged();
            });

            await hubConnection.StartAsync();
            await hubConnection.SendAsync("JoinRoom", RoomName);

            if (!AppState.RoomNames.Contains(RoomName))
            {
                AppState.RoomNames.Add(RoomName);
                AppState.AppStateUpdated?.Invoke();
            }
        }

        private async Task Send()
        {
            await hubConnection?.SendAsync("SendMessage", RoomName, newChatMessage);
            newChatMessage.Message = string.Empty;
        }

        public bool IsConnected => hubConnection?.State == HubConnectionState.Connected;

        public async ValueTask DisposeAsync()
        {
            if (hubConnection != null)
            {
                await hubConnection.DisposeAsync();
            }
        }
    }
}
