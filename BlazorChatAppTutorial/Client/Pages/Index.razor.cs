namespace BlazorChatAppTutorial.Client.Pages
{
    public partial class Index
    {
        private readonly AppState appState = new();
        bool formSaved = false;

        string newRoomName;
        string roomJoinedMessage;

        protected override void OnInitialized()
        {
            appState.UserName = AppState.UserName;
            appState.RoomNames.AddRange(AppState.RoomNames);
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
                AppState.RoomNames.Add(newRoomName);
                roomJoinedMessage = $"Joined {newRoomName}!";
                newRoomName = string.Empty;
                AppState.AppStateUpdated?.Invoke();
            }
        }
    }
}
