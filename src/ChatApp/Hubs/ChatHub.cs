using ChatApp.Exceptions;
using ChatApp.Lib.Messaging.Persistence;
using ChatApp.Lib.Users.Model;
using ChatApp.Lib.Users.Persistence;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Linq;
using System.Threading.Tasks;
using ChatMessage = ChatApp.Lib.Messaging.Model.ChatMessage;

namespace ChatApp.Hubs
{
    /// <summary>
    /// Hub for all chat based SignalR functionality
    /// </summary>
    public class ChatHub : Hub
    {
        private readonly IChatMessageRepository _chatMessageRepository;
        private readonly IActiveUserRepository _activeUserRepository;

        private const string NewMessageMethod = "newMessage";
        private const string UserLeftMethod = "userLeft";
        private const string NewUserMethod = "newUser";

        public ChatHub(
            IChatMessageRepository chatMessageRepository,
            IActiveUserRepository activeUserRepository)
        {
            _chatMessageRepository = chatMessageRepository;
            _activeUserRepository = activeUserRepository;
        }

        /// <summary>
        /// Register new chat handle and associate it with the connection id.
        /// Exception is thrown if handle is in use or invalid
        /// </summary>
        /// <param name="handle">Handle to register</param>
        public async Task RegisterHandle(string handle)
        {
            if (string.IsNullOrWhiteSpace(handle)) {
                throw new ChatHubException("Illegal handle");
            }

            var activeUsers = await _activeUserRepository.GetActiveUsers();

            if (activeUsers.Select(u => u.Handle).Contains(handle)) {
                throw new ChatHubException("Handle is taken");
            }

            await Clients.All.SendAsync(NewUserMethod, handle);

            await _activeUserRepository.AddActiveUser(new User
            {
                Id = Context.ConnectionId,
                Handle = handle
            });
        }

        /// <summary>
        /// Post a new message. Connection Id is used to find handle for the message, if no handle is found a excetion is thrown.
        /// </summary>
        /// <param name="message">Message to send, can be null which results in empty message</param>
        public async Task PostMessage(string message)
        {
            var handle = await _activeUserRepository.GetHandleForId(Context.ConnectionId);

            if (string.IsNullOrWhiteSpace(handle)) {
                throw new ChatHubException("Connection has no associated handle");
            }

            var chatMessage = new ChatMessage
            {
                Id = Guid.NewGuid(),
                Handle = handle,
                Message = message ?? "-",
                Timestamp = DateTimeOffset.UtcNow
            };

            await _chatMessageRepository.InsertMessage(chatMessage);

            await Clients.All.SendAsync(NewMessageMethod, chatMessage);
        }

        /// <summary>
        /// OnDisconnectedAsync override to handle user leaving (status and broadcast)
        /// </summary>
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var handle = await _activeUserRepository.RemoveUser(Context.ConnectionId);
            if (!string.IsNullOrWhiteSpace(handle)) {
                await Clients.All.SendAsync(UserLeftMethod, handle);
            }

            await base.OnDisconnectedAsync(exception);
        }
    }
}
