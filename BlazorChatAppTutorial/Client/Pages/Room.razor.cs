using BlazorChatAppTutorial.Shared.Models;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace BlazorChatAppTutorial.Client.Pages
{
    [Route("/room/{RoomName}")]
    public partial class Room
    {
        [Inject] private NavigationManager NavigationManager { get; set; }

        [Inject] private HttpClient HttpClient { get; set; }

        [Parameter] public string RoomName { get; set; }

        private readonly List<ChatMessageModel> ChatMessages = new();
        private readonly ChatMessageModel newChatMessage = new();

        protected override async Task OnParametersSetAsync()
        {
            newChatMessage.UserName = AppState.UserName;
            ChatMessages.Clear();
            AppState.CurrentRoom = this;

            List<ChatMessageModel> previousChatMessages = (await HttpClient.GetFromJsonAsync<IEnumerable<ChatMessageModel>>($"chat/{RoomName}")).ToList();
            if (previousChatMessages?.Any() ?? false)
            {
                ChatMessages.AddRange(previousChatMessages);
                StateHasChanged();
            }

            await AppState.SetupHubConnection(RoomName);

            if (!AppState.RoomNames.Contains(RoomName))
            {
                AppState.RoomNames.Add(RoomName);
                AppState.AppStateUpdated?.Invoke();
            }
        }

        public void ReceiveMessage(ChatMessageModel chatMessage)
        {
            ChatMessages.Add(chatMessage);
            StateHasChanged();
        }

        private async Task Send()
        {
            await AppState.SendAsync(RoomName, newChatMessage);
            newChatMessage.Message = string.Empty;
        }

        private bool IsConnected => AppState.IsRoomHubConnected(RoomName);
    }
}
