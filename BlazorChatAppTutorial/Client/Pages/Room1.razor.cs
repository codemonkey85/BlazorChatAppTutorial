using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorChatAppTutorial.Client.Pages
{
    [Route("/room1")]
    public partial class Room1 : IDisposable
    {
        [Inject] NavigationManager NavigationManager { get; set; }

        private HubConnection hubConnection;
        private List<string> messages = new();
        private string userInput;
        private string messageInput;

        protected override async Task OnInitializedAsync()
        {
            hubConnection = new HubConnectionBuilder()
                .WithUrl(NavigationManager.ToAbsoluteUri("/chathub"))
                .Build();

            hubConnection.On<string, string>("ReceiveMessage", (user, message) =>
            {
                string encodedMsg = $"{user}: {message}";
                messages.Add(encodedMsg);
                StateHasChanged();
            });

            await hubConnection.StartAsync();
        }

        Task Send() => hubConnection.SendAsync("SendMessage", userInput, messageInput);

        public bool IsConnected => hubConnection.State == HubConnectionState.Connected;

        public void Dispose() => _ = hubConnection.DisposeAsync();
    }
}
