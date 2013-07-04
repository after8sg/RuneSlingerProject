
using System.Collections.Generic;
using Assets.Code.Abstract;
namespace Assets.Code
{
	public static class RuneTextureMap
	{
        private static readonly Dictionary<RuneType, List<ITextureAssigner>> SlotSymbolTextureMap;
        private static readonly Dictionary<RuneType, List<ITextureAssigner>> RuneSymbolTextureMap;

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

        }

        private static ITextureAssigner NumericSlotSprite(int x, int y)
        {
            return new SpriteTextureAssigner("RuneNumbersSlot", x, y, 128, 128);
        }

        private static ITextureAssigner NumericRuneSprite(int x, int y)
        {
            return new SpriteTextureAssigner("RuneNumbers", x, y, 128, 128);
        }

        public static ITextureAssigner GetSlotAssigner(RuneType type, uint typeIndex)
        {
            return SlotSymbolTextureMap[type][(int)typeIndex];
        }

        public static ITextureAssigner GetRuneAssigner(RuneType type, uint typeIndex)
        {
            return RuneSymbolTextureMap[type][(int)typeIndex];
        }

	}
}
