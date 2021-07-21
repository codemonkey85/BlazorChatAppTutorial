using BlazorChatAppTutorial.Shared.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorChatAppTutorial.Client.Pages
{
    public partial class Index
    {
        private readonly AppState appState = new(null);
        bool formSaved = false;

        private readonly ChatRoomModel newRoom = new();

        string roomJoinedMessage;

        protected override void OnInitialized()
        {
            appState.UserName = AppState.UserName;
        }

        private void OnValidFormSubmitUserName()
        {
            AppState.UserName = appState.UserName;
            formSaved = true;
            AppState.AppStateUpdated?.Invoke();
        }

        private async Task OnValidFormSubmitRoomNames()
        {
            newRoom.RoomName = newRoom.RoomName.Trim();
            if (AppState.JoinedRooms.Select(chatRoom => chatRoom.Value.RoomName).Contains(newRoom.RoomName))
            {
                roomJoinedMessage = $"Already in {newRoom.RoomName}.";
            }
            else
            {
                AppState.JoinedRooms.Add(newRoom.RoomName, new() { RoomName = newRoom.RoomName });

                if (AppState.JoinedRooms.TryGetValue(newRoom.RoomName, out ChatRoomModel createdRoom))
                {
                    await AppState.SetupHubConnection(createdRoom);
                }

                roomJoinedMessage = $"Joined {newRoom.RoomName}!";
                newRoom.RoomName = string.Empty;
            }
        }
    }
}
