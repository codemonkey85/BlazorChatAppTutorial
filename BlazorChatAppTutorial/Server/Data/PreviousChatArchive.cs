using BlazorChatAppTutorial.Shared.Models;
using System.Collections.Generic;

namespace BlazorChatAppTutorial.Server.Data
{
    public class PreviousChatArchive
    {
        public IDictionary<string, IList<ChatMessageModel>> Chats { get; set; } = new Dictionary<string, IList<ChatMessageModel>>();
    }
}
