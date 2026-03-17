using UnityEngine;

using utils;

namespace Obstacles.Tall_Obstacle.Scripts
{
    public class TallObstacleBehaviour : MonoBehaviour, IInteractive
    {
        public void OnCollision(GameObject _)
        {
            Debug.Log("GAME OVER");
        }

        public void OnTrigger(GameObject _) {}
    }
}
