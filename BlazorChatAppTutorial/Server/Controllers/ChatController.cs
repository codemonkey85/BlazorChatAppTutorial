using BlazorChatAppTutorial.Server.Data;
using BlazorChatAppTutorial.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace BlazorChatAppTutorial.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ChatController : ControllerBase
    {
        public PreviousChatArchive PreviousChatArchive { get; }

        private ILogger<ChatController> Logger { get; }

        public ChatController(PreviousChatArchive previousChatArchive, ILogger<ChatController> logger)
        {
            PreviousChatArchive = previousChatArchive;
            Logger = logger;
        }

        [HttpGet("{roomName}")]
        public IEnumerable<ChatMessageModel> Get([FromRoute] string roomName)
        {
            return PreviousChatArchive.Chats.TryGetValue(roomName, out IList<ChatMessageModel> archive) ? archive : Array.Empty<ChatMessageModel>();
        }
    }
}
