
using Assets.Code.Abstract;
using UnityEngine;
namespace Assets.Code
{
	public static class GameObjectHelpers
	{
        public static void CreateSprite(GameObject gameObject,ITextureAssigner textureAssigner)
        {
            CreateSpriteMesh(gameObject);
            textureAssigner.Assign(gameObject.GetComponent<MeshRenderer>(),gameObject.GetComponent<MeshFilter>());
        }

        public static void CreateSpriteMesh(GameObject gameObject)
        {
            var filter = gameObject.AddComponent<MeshFilter>();
            var renderer = gameObject.AddComponent<MeshRenderer>();

            // 1 --- 2
            // |      |
            // 0 --- 3
            var verts = new[]
            {
                new Vector3(0,0),
                new Vector3(0,1),
                new Vector3(1,1),
                new Vector3(1,0)
            };

            var mesh = new Mesh();
            filter.mesh = mesh;
            mesh.vertices = verts;
            mesh.triangles = new[]
            {
                0,1,2,2,3,0
            };
            mesh.RecalculateNormals();

            renderer.material = (Material)Resources.Load("SpriteMaterial");
            
        }
	}
}
