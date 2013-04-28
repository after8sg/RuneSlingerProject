
namespace RuneSlinger.Base.ValueObjects
{
    public class LobbySession
    {
        public uint Id { get; private set;}
        public string Username { get; private set;}

        public LobbySession(uint id, string username)
        {
            Id = id;
            Username = username;
        }
    }

}
