
using Assets.Code.Abstract;
using UnityEngine;
using System.Collections.Generic;
namespace Assets.Code
{
	public class BasicTextureAssigner : ITextureAssigner
	{
        private static readonly Dictionary<string, Texture> TextureCache;
        private readonly Texture _texture;

        static BasicTextureAssigner()
        {
            TextureCache = new Dictionary<string, Texture>();
        }

        public BasicTextureAssigner(string textureResource)
        {
            Texture output;
            if (TextureCache.TryGetValue(textureResource, out output))
                _texture = output;
            else
                TextureCache[textureResource] = _texture = (Texture)Resources.Load(textureResource);
        }

        public void Assign(MeshRenderer renderer, MeshFilter filter)
        {
            renderer.material.mainTexture = _texture;

            // 1 --- 2
            // |      |
            // 0 --- 3
            filter.mesh.uv = new[]
            {
                new Vector2(0,0),
                new Vector2(0,1),
                new Vector2(1,1),
                new Vector2(1,0)
            };
        }

        
    }
}
