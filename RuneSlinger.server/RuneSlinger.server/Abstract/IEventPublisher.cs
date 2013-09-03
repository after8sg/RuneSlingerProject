
using RuneSlinger.Base.Abstract;
using System.Collections.Generic;
namespace RuneSlinger.server.Abstract
{
    public interface IEventPublisher
    {
        void Commit();
        void Publish(IEvent @event,INetworkedSession session);
        void Publish(IEvent @event, IEnumerable<INetworkedSession> sessions);
    }
}
