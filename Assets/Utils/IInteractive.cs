using UnityEngine;

namespace Utils
{
    public interface IInteractive
    {
        // for OnCollisionEnter
        void OnCollision(GameObject _) {}

        // for OnTriggerEnter
        void OnTrigger(GameObject _) {}
        
        // for RayCasting
        void OnRayCastHit(GameObject _) {}
    }
}
