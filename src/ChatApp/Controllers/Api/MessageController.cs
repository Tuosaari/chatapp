using ChatApp.Lib.Messaging.Model;
using ChatApp.Lib.Messaging.Persistence;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChatApp.Controllers.Api
{
    /// <summary>
    /// Controller for providing <see cref="ChatMessage">chat messages</see>
    /// </summary>
    [Route("api/v1/messages")]
    public class MessageController : Controller
    {
        private readonly IChatMessageRepository _chatMessageRepository;
        //Limit for max messages to fetch
        private const int MaxMessages = 1000;

        public MessageController(IChatMessageRepository chatMessageRepository)
        {
            _chatMessageRepository = chatMessageRepository;
        }

        /// <summary>
        /// Get all <see cref="ChatMessage">chat messages</see> available in the provided repository
        /// TODO: Paging support
        /// </summary>
        /// <returns>
        /// <see cref="IEnumerable{ChatMessage}"/> containing all messages in repository (count capped by MaxMessages const).
        /// Enumerable is empty if no messages exist
        /// </returns>
        [HttpGet("all")]
        public async Task<IEnumerable<ChatMessage>> GetAllMessages()
        {
            return await _chatMessageRepository.GetMessages(MaxMessages);
        }
    }
}
