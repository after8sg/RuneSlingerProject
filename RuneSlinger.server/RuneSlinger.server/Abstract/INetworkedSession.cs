
using RuneSlinger.Base.Abstract;
namespace RuneSlinger.server.Abstract
{
    public interface INetworkedSession
    {
        Registry Registry { get; }

        void Publish(IEvent @event);
    }
}
