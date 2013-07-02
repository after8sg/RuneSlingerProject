
using Assets.Code.ValueObjects;
namespace Assets.Code.Abstract
{
	public interface IGameView
	{
        bool CanPlaceRune(Rune rune, RuneSlot slot);
	}
}
