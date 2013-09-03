
using System.Collections.Generic;
using Assets.Code.Abstract;
using RuneSlinger.Base.ValueObjects;
namespace Assets.Code
{
	public static class RuneTextureMap
	{
        private static readonly Dictionary<RuneType, List<ITextureAssigner>> SlotSymbolTextureMap;
        private static readonly Dictionary<RuneType, List<ITextureAssigner>> RuneSymbolTextureMap;
        private static readonly Dictionary<RuneType, List<ITextureAssigner>> DroppedSymbolTextureMap;

        static RuneTextureMap()
        {
            SlotSymbolTextureMap = new Dictionary<RuneType, List<ITextureAssigner>>
            {
                {
                    RuneType.Numeric,
                    new List<ITextureAssigner>
                    {
                        NumericSlotSprite(1,1),     //0
                        NumericSlotSprite(0,3),
                        NumericSlotSprite(1,3),
                        NumericSlotSprite(2,3),
                        NumericSlotSprite(3,3),
                        NumericSlotSprite(0,2),
                        NumericSlotSprite(1,2),
                        NumericSlotSprite(2,2),
                        NumericSlotSprite(3,2),
                        NumericSlotSprite(0,1)      //9
                    }
                },
                {
                    RuneType.Symbolic,
                    new List<ITextureAssigner>
                    {
                    }
                }
            };

            RuneSymbolTextureMap = new Dictionary<RuneType, List<ITextureAssigner>>
            {
                {
                    RuneType.Numeric,
                    new List<ITextureAssigner>
                    {
                        NumericRuneSprite(1,1),     //0
                        NumericRuneSprite(0,3),
                        NumericRuneSprite(1,3),
                        NumericRuneSprite(2,3),
                        NumericRuneSprite(3,3),
                        NumericRuneSprite(0,2),
                        NumericRuneSprite(1,2),
                        NumericRuneSprite(2,2),
                        NumericRuneSprite(3,2),
                        NumericRuneSprite(0,1)      //9
                    }
                },
                {
                    RuneType.Symbolic,
                    new List<ITextureAssigner>
                    {
                    }
                }
            };

            DroppedSymbolTextureMap = new Dictionary<RuneType, List<ITextureAssigner>>
            {
                {
                    RuneType.Numeric,
                    new List<ITextureAssigner>
                    {
                        NumericDroppedSprite(1,1),     //0
                        NumericDroppedSprite(0,3),
                        NumericDroppedSprite(1,3),
                        NumericDroppedSprite(2,3),
                        NumericDroppedSprite(3,3),
                        NumericDroppedSprite(0,2),
                        NumericDroppedSprite(1,2),
                        NumericDroppedSprite(2,2),
                        NumericDroppedSprite(3,2),
                        NumericDroppedSprite(0,1)      //9
                    }
                },
                {
                    RuneType.Symbolic,
                    new List<ITextureAssigner>
                    {
                    }
                }
            };

        }
        
        private static ITextureAssigner NumericSlotSprite(int x, int y)
        {
            return new SpriteTextureAssigner("RuneNumbersSlot", x, y, 128, 128);
        }

        private static ITextureAssigner NumericRuneSprite(int x, int y)
        {
            return new SpriteTextureAssigner("RuneNumbers", x, y, 128, 128);
        }

        private static ITextureAssigner NumericDroppedSprite(int x, int y)
        {
            return new SpriteTextureAssigner("RuneNumbersDropped", x, y, 128, 128);
        }

        public static ITextureAssigner GetSlotAssigner(RuneType type, uint typeIndex)
        {
            return SlotSymbolTextureMap[type][(int)typeIndex];
        }

        public static ITextureAssigner GetRuneAssigner(RuneType type, uint typeIndex)
        {
            return RuneSymbolTextureMap[type][(int)typeIndex];
        }

        public static ITextureAssigner GetDroppedAssigner(RuneType type, uint typeIndex)
        {
            return DroppedSymbolTextureMap[type][(int)typeIndex];
        }

	}
}
