using BlazorChatAppTutorial.Shared.Models;
using Microsoft.AspNetCore.Components;

namespace BlazorChatAppTutorial.Client.Components
{
    public partial class ChatMessageComponent
    {
        [Parameter]
        public ChatMessageModel ChatMessage { get; set; }
    }
}
