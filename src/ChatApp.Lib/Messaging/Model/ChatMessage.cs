using System;

namespace ChatApp.Lib.Messaging.Model
{
    /// <summary>
    /// Represents a single message posted by chat user
    /// </summary>
    public class ChatMessage
    {
        /// <summary>
        /// Auto-generated Id for message
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Chat handle (name entered by user)
        /// </summary>
        public string Handle { get; set; }

        /// <summary>
        /// Actual message text 
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Server timestamp of the message
        /// </summary>
        public DateTimeOffset Timestamp { get; set; }
    }
}
