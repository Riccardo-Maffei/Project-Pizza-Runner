using utils;
using UnityEngine;


namespace Collectibles.Scripts
{
    public interface ICollectible {}

    public abstract class CollectibleBase: MonoBehaviour, ICollectible, IInteractive
    {
        protected abstract void TriggerBehaviour();

        public void OnTrigger(GameObject _)
        {
            TriggerBehaviour();
            Destroy(gameObject);
        }
    }
}
