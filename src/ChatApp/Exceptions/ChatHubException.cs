using System;

namespace ChatApp.Exceptions
{
    /// <summary>
    /// Custom exception used in all error cases originating from ChatHub
    /// </summary>
    public class ChatHubException : Exception
    {
        public ChatHubException(string message) : base(message)
        {
        }
    }
}
