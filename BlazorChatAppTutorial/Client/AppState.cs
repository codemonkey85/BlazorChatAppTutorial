using System;
using System.Collections.Generic;

namespace BlazorChatAppTutorial.Client
{
    public class AppState
    {
        public string UserName { get; set; }

        public List<string> RoomNames { get; set; } = new List<string>();

        public Action AppStateUpdated { get; set; }
    }
}
