
namespace Assets.Code
{
	public class GameSession
	{

        public string Username { get; private set; }
        public uint UserId { get; private set; }

        public GameSession(string username, uint userId)
        {
            Username = username;
            UserId = userId;
        }

    }
}
