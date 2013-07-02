using UnityEngine;
using Assets.Code;
using Assets.Code.ValueObjects;
using Assets.Code.Abstract;

public class GameTestManager : MonoBehaviour 
{

    class MockGameView : IGameView
    {
        public bool CanPlaceRune(Rune rune, RuneSlot slot)
        {
            return rune.Type == slot.Type && rune.RuneTypeIndex == slot.RuneTypeIndex;
        }
    }

	// Use this for initialization
	public void Start () 
    {
        var game = GameObjectFactory.Create<RuneGameGO>("Game");
        var ourSession = new GameSession("Player 1",1);
        const int width = 5;
        const int height = 10;
        const int totalsize = width * height;

        game.Initialize(
            new[] { ourSession, new GameSession("Player 2", 2) },
            ourSession,
            new MockGameView(),
            width,
            height
            );

        

        uint slotTypeIndex = 0;
        for (uint x = 0; x < width; x++)
        {
            for (uint y = 0; y < height; y++)
            {
                var runeIndex = slotTypeIndex++ % 9;
                game.AddSlot(x, y, new RuneSlot(slotTypeIndex,RuneType.Numeric,runeIndex));
                game.AddRune(slotTypeIndex < (totalsize /2) ? GutterLocation.Left : GutterLocation.Right,new Rune(slotTypeIndex,RuneType.Numeric,runeIndex));
                
            }
        }

        game.Start();
	}
	
}
