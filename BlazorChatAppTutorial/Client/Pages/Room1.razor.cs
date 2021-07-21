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
    [Route("/room1")]
    public partial class Room1 : IDisposable
    {
        [Inject] NavigationManager NavigationManager { get; set; }

        [Inject] HttpClient HttpClient { get; set; }

        private HubConnection hubConnection;
        private IList<string> messages = new List<string>();
        private string userInput;
        private string messageInput;

        protected override async Task OnInitializedAsync()
        {
            List<ChatModel> previousChatMessages = (await HttpClient.GetFromJsonAsync<IEnumerable<ChatModel>>("chat/room1")).ToList();
            foreach (ChatModel chatMessage in previousChatMessages)
            {
                messages.Add($"{chatMessage.UserName}: {chatMessage.Message}");
            }
            StateHasChanged();

            hubConnection = new HubConnectionBuilder()
                .WithUrl(NavigationManager.ToAbsoluteUri("/chathub"))
                .Build();

            hubConnection.On<string, string>("ReceiveMessage", (userName, message) =>
            {
                messages.Add($"{userName}: {message}");
                StateHasChanged();
            });

            await hubConnection.StartAsync();
        }

        Task Send() => hubConnection?.SendAsync("SendMessage", "room1", userInput, messageInput);

        public bool IsConnected => hubConnection?.State == HubConnectionState.Connected;

        public void Dispose() => hubConnection?.DisposeAsync();
    }
}
