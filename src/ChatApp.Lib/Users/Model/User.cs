namespace ChatApp.Lib.Users.Model
{
    /// <summary>
    /// Class that represents a single user in chat
    /// </summary>
    public class User
    {
        /// <summary>
        /// Unique Id of the user e.g. SignalR connection id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Handle entered by the user
        /// </summary>
        public string Handle { get; set; }
    }
}
