using UnityEngine;

namespace utils
{
    public interface IInteractive
    {
        // for OnCollisionEnter
        void OnCollision(GameObject _) {}

        // for OnTriggerEnter
        void OnTrigger(GameObject _) {}
    }
}
