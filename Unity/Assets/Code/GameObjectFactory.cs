using UnityEngine;

namespace Assets.Code
{
    public static class GameObjectFactory
    {
        public static TComponent Create<TComponent>(string name = "<Empty Game Object>") where TComponent : Component
        {
            var gameObject = new GameObject(name);
            return gameObject.AddComponent<TComponent>();
        }
    }

}

