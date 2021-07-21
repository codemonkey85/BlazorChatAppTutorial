using BlazorChatAppTutorial.Shared.Models;
using System.Collections.Generic;

namespace BlazorChatAppTutorial.Server.Data
{
    public class PreviousChatArchive
    {
        // public PreviousChatArchive() { }

        public IDictionary<string, IList<ChatModel>> Chats { get; set; } = new Dictionary<string, IList<ChatModel>>();
    }
}
