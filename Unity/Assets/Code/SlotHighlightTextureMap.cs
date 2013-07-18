
using System.Collections.Generic;
using Assets.Code.ValueObjects;
using Assets.Code.Abstract;
namespace Assets.Code
{
	public static class SlotHighlightTextureMap
	{
        private static readonly Dictionary<SlotHighlightType, ITextureAssigner> SlotHighlightTextures;

        static SlotHighlightTextureMap()
        {
            SlotHighlightTextures = new Dictionary<SlotHighlightType, ITextureAssigner>
            {
                { SlotHighlightType.Hover, new SpriteTextureAssigner("TileHighlights",1,0,128,128)}
            };
        }

        public static ITextureAssigner GetHighlightTextureAssigner(SlotHighlightType type)
        {
            return SlotHighlightTextures[type];
        }
	}
}
