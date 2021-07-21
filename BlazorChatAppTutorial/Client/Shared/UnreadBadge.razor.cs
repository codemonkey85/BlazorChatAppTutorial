using Microsoft.AspNetCore.Components;

namespace BlazorChatAppTutorial.Client.Shared
{
    public partial class UnreadBadge
    {
        [Parameter] public int UnreadCount { get; set; }
    }
}
