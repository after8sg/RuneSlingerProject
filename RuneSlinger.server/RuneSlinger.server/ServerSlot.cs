
using RuneSlinger.Base.ValueObjects;
using RuneSlinger.server.Abstract;
namespace RuneSlinger.server
{
    public class ServerSlot
    {
        public RuneSlot Slot { get; private set; }
        public INetworkedSession PlacedBy { get; set; }

        public ServerSlot(RuneSlot slot)
        {
            Slot = slot;
        }
    }
}
