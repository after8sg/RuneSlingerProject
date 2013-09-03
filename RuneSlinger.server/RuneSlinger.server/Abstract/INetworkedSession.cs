
using RuneSlinger.Base.Abstract;
using RuneSlinger.server.Components;
namespace RuneSlinger.server.Abstract
{
    public interface INetworkedSession
    {
        SessionAuth Auth { get; }
        Registry Registry { get; }

        void Authenticate(SessionAuth auth);
    }
}
