
using RuneSlinger.Base.ValueObjects;
namespace Assets.Code.Abstract
{
	public interface IGameCommands
	{
        void PlaceRune(Rune rune, RuneSlot slot);
	}
}
