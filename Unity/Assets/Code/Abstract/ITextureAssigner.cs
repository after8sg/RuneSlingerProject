
using UnityEngine;
namespace Assets.Code.Abstract
{
	public interface ITextureAssigner
	{
        void Assign(MeshRenderer renderer, MeshFilter filter);
	}
}
