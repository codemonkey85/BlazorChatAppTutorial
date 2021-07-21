using System;
using System.ComponentModel.DataAnnotations;

namespace BlazorChatAppTutorial.Shared.Models
{
    public class ChatMessageModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Message { get; set; }

        public DateTime DateSent { get; set; }
    }
}
