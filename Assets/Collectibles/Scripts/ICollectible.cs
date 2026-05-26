using Utils;
using UnityEngine;


namespace Collectibles.Scripts
{
    public interface ICollectible {}

    public abstract class CollectibleBase: MonoBehaviour, ICollectible, IInteractive
    {
        public AudioClip collectSound;
        
        protected abstract void TriggerBehaviour();

        public void OnTrigger(GameObject _)
        {
            TriggerBehaviour();
            
            AudioSource.PlayClipAtPoint(collectSound, transform.position);
            
            Destroy(gameObject);
        }
    }
}
