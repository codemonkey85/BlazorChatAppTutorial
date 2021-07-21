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
    [Route("/room2")]
    public partial class Room2 : IDisposable
    {
        [Inject] NavigationManager NavigationManager { get; set; }

        [Inject] HttpClient HttpClient { get; set; }

        private HubConnection hubConnection;
        private IList<string> messages = new List<string>();
        private string userInput;
        private string messageInput;

        protected override async Task OnInitializedAsync()
        {
            IList<ChatModel> previousChatMessages = (await HttpClient.GetFromJsonAsync<ChatModel[]>("chat/room2")).ToList();
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

        Task Send() => hubConnection?.SendAsync("SendMessage", "room2", userInput, messageInput);

        public bool IsConnected => hubConnection?.State == HubConnectionState.Connected;

        public void Dispose() => hubConnection?.DisposeAsync();
    }
}