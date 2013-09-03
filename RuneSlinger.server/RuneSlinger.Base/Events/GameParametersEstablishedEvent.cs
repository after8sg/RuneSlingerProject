
using RuneSlinger.Base.Abstract;
namespace RuneSlinger.Base.Events
{
    public class GameParametersEstablishedEvent : IEvent
    {
        public uint BoardWidth { get; private set; }
        public uint BoardHeight { get; private set; }

        public GameParametersEstablishedEvent(uint boardWidth, uint boardHeight)
        {
            BoardWidth = boardWidth;
            BoardHeight = boardHeight;
        }


        
    }
}
