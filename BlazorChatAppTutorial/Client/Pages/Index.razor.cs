namespace BlazorChatAppTutorial.Client.Pages
{
    public partial class Index
    {
        private readonly AppState appState = new();

        protected override void OnInitialized()
        {
            appState.UserName = AppState.UserName;
            appState.RoomNames.AddRange(AppState.RoomNames);
        }

        private void OnValidFormSubmit()
        {
            AppState.UserName = appState.UserName;
            AppState.RoomNames.Clear();
            AppState.RoomNames.AddRange(appState.RoomNames);
        }
    }
}
