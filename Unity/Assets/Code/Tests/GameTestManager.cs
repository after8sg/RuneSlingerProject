using UnityEngine;
using Assets.Code;
using Assets.Code.ValueObjects;
using Assets.Code.Abstract;

public class GameTestManager : MonoBehaviour 
{

    class MockGameView : IGameCommands
    {
        private readonly RuneGameGO _game;
        public MockGameView(RuneGameGO game)
        {
            _game = game;
        }

        public void PlaceRune(Rune rune, RuneSlot slot)
        {
            if (rune.Type == slot.Type && rune.RuneTypeIndex == slot.RuneTypeIndex)
                _game.RunePlaced(rune.Id, slot.Id);
            else
                _game.PlacementRejected(rune.Id, slot.Id);

        }

    }

	// Use this for initialization
	public void Start () 
    {
        var game = GameObjectFactory.Create<RuneGameGO>("Game");
        var ourSession = new GameSession("Player 1",1);
        const int boardwidth = 2;
        const int boardheight = 2;
        const int totalsize = boardwidth * boardheight;

        game.Initialize(
            new[] { ourSession, new GameSession("Player 2", 2) },
            ourSession,
            new MockGameView(game),
            boardwidth,
            boardheight,
            10
            );

        

        uint slotTypeIndex = 0;
        for (uint x = 0; x < boardwidth; x++)
        {
            for (uint y = 0; y < boardheight; y++)
            {
                var runeIndex = slotTypeIndex++ % 9;
                game.AddSlot(x, y, new RuneSlot(slotTypeIndex,RuneType.Numeric,runeIndex));
                game.AddRune(slotTypeIndex <= totalsize /2 ? GutterLocation.Left : GutterLocation.Right,new Rune(slotTypeIndex,RuneType.Numeric,runeIndex));
                
            }
        }

        game.Start();
	}
	
}
