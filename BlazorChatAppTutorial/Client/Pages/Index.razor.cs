using BlazorChatAppTutorial.Shared.Models;

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

            foreach (string roomName in AppState.RoomNames)
            {
                appState.TryAddRoom(roomName);
            }
        }

        private void OnValidFormSubmitUserName()
        {
            AppState.UserName = appState.UserName;
            formSaved = true;
            AppState.AppStateUpdated?.Invoke();
        }

        private void OnValidFormSubmitRoomNames()
        {
            if (AppState.RoomNames.Contains(newRoom.RoomName))
            {
                roomJoinedMessage = $"Already in {newRoom.RoomName}.";
            }
            else
            {
                AppState.TryAddRoom(newRoom.RoomName);
                AppState.AppStateUpdated?.Invoke();
                roomJoinedMessage = $"Joined {newRoom.RoomName}!";
                newRoom.RoomName = string.Empty;
            }
        }
    }
}
