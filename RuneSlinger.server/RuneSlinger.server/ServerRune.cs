

using RuneSlinger.Base.ValueObjects;
using RuneSlinger.server.Abstract;
namespace RuneSlinger.server
{
    public class ServerRune 
    {
        public Rune Rune { get; private set; }
        public INetworkedSession PlacedBy { get; set; }

        public ServerRune(Rune rune)
        {
            Rune = rune;
        }

    }
}
