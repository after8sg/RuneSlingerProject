
namespace RuneSlinger.server.Components
{
    public class SessionAuth
    {
        public uint Id { get; private set; }
        public string Username { get; private set; }
        public string Email { get; private set; }
        public bool IsAuthenticated { get; private set; }

        public SessionAuth(uint id, string username, string email)
        {
            Id = id;
            Username = username;
            Email = email;
            IsAuthenticated = true;
        }

        public SessionAuth()
        {
            IsAuthenticated = false;
        }

    }
}
