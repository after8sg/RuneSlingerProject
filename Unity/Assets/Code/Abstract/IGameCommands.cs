
using Assets.Code.ValueObjects;
namespace Assets.Code.Abstract
{
	public interface IGameCommands
	{
        void PlaceRune(Rune rune, RuneSlot slot);
	}
}
