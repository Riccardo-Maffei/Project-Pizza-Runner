using UnityEngine;

namespace utils
{
    public interface IInteractive
    {
        // for OnCollisionEnter
        void OnCollision(GameObject player);

        // for OnTriggerEnter
        void OnTrigger(GameObject player);
    }
}

