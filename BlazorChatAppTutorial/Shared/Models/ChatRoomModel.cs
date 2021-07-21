using System.ComponentModel.DataAnnotations;

namespace BlazorChatAppTutorial.Shared.Models
{
    public class ChatRoomModel
    {
        [Required]
        public string RoomName { get; set; }
    }
}
