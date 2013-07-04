
using Assets.Code.Abstract;
using UnityEngine;
using System.Collections.Generic;
namespace Assets
{
	public class SpriteTextureAssigner : ITextureAssigner
	{
        private readonly static Dictionary<string, Texture> TextureCache;

        static SpriteTextureAssigner()
        {
            TextureCache = new Dictionary<string, Texture>();
        }

        private readonly Texture _texture;
        private readonly int _spriteX;
        private readonly int _spriteY;
        private readonly float _spriteWidth;
        private readonly float _spriteHeight;

        public SpriteTextureAssigner(string spriteMapTexture, int spriteX, int spriteY, float spritewidth, float spriteheight)
        {
           
            _spriteX = spriteX;
            _spriteY = spriteY;
            _spriteWidth = spritewidth;
            _spriteHeight = spriteheight;

            if (!TextureCache.TryGetValue(spriteMapTexture, out _texture))
                _texture = TextureCache[spriteMapTexture] = (Texture)Resources.Load(spriteMapTexture);
        }

        public void Assign(MeshRenderer renderer, MeshFilter filter)
        {
            // 1 -- 2
            // |     |
            // 0 -- 3
            renderer.material.mainTexture = _texture;

            var spritePerRow = _texture.width / _spriteWidth;
            var spritePerCol = _texture.height / _spriteHeight;

            var xScale = 1 / spritePerRow;
            var yScale = 1 / spritePerCol;

            var spriteX = _spriteX * xScale;
            var spriteY = _spriteY * yScale;

            filter.mesh.uv = new[]
            {
                new Vector2(spriteX,spriteY),
                new Vector2(spriteX,spriteY + yScale),
                new Vector2(spriteX + xScale, spriteY + yScale),
                new Vector2(spriteX + xScale, spriteY)
                
            };

        }


        
    }
}
