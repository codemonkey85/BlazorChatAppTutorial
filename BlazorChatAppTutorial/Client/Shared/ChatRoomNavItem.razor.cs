using BlazorChatAppTutorial.Shared.Models;
using Microsoft.AspNetCore.Components;

namespace BlazorChatAppTutorial.Client.Shared
{
    public partial class ChatRoomNavItem
    {
        [Parameter] public ChatRoomModel JoinedRoom { get; set; }
    }
}
