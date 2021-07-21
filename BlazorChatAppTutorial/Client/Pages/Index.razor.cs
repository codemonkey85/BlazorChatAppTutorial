namespace BlazorChatAppTutorial.Client.Pages
{
    public partial class Index
    {
        private readonly AppState appState = new(null);
        bool formSaved = false;

        string newRoomName;
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
            if (AppState.RoomNames.Contains(newRoomName))
            {
                roomJoinedMessage = $"Already in {newRoomName}.";
            }
            else
            {
                AppState.TryAddRoom(newRoomName);
                AppState.AppStateUpdated?.Invoke();
                roomJoinedMessage = $"Joined {newRoomName}!";
                newRoomName = string.Empty;
            }
        }
    }
}
