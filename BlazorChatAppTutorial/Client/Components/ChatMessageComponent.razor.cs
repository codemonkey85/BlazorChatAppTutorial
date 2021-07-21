using BlazorChatAppTutorial.Shared.Models;
using Microsoft.AspNetCore.Components;

namespace BlazorChatAppTutorial.Client.Components
{
    public partial class ChatMessageComponent
    {
        [Parameter]
        public ChatMessageModel ChatMessage { get; set; }

        [Parameter] public bool IsOutgoing { get; set; }

        private string MessageBody =>
            $"{ChatMessage.UserName} {ChatMessage.DateSent:T}: {ChatMessage.Message}";

        private string MessageClass => "talk-bubble tri-right round right-in";
        private string MessageBodyClass => "talktext";
    }
}
