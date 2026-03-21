using UnityEngine;

using utils;


namespace Obstacles.Tall_Obstacle.Scripts
{
    public class TallObstacleBehaviour : MonoBehaviour, IInteractive
    {
        public void OnCollision(GameObject _)
        {
            if (GameData.Hp > 0) GameData.Hp -= 1;
            else Debug.Log("GAME OVER");
        }

        public void OnTrigger(GameObject _) {}
    }
}
