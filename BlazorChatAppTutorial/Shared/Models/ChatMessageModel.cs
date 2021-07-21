using System;

namespace BlazorChatAppTutorial.Shared.Models
{
    public class ChatMessageModel
    {
        public string UserName { get; set; }

        public string Message { get; set; }

        public DateTime DateSent { get; set; }
    }
}
