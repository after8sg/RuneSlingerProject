using RuneSlinger.Base.Abstract;

namespace Assets.Code.Abstract
{
	public interface IEventHandler<TEvent> where TEvent : IEvent
	{
        void Handle(TEvent @event);
        
	}
}
