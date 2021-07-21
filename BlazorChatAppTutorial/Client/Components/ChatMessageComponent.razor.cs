using BlazorChatAppTutorial.Shared.Models;
using Microsoft.AspNetCore.Components;

namespace BlazorChatAppTutorial.Client.Components
{
    public partial class ChatMessageComponent
    {
        [Parameter]
        public ChatModel ChatMessage { get; set; }
    }
}
