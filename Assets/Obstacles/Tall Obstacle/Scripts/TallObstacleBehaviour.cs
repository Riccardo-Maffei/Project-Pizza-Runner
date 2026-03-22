using UnityEngine;

using utils;


namespace Obstacles.Tall_Obstacle.Scripts
{
    public class TallObstacleBehaviour : MonoBehaviour, IInteractive
    {
        public void OnCollision(GameObject _)
        {
            //if (GameData.Hp.GetValue() > 0) GameData.Hp.Decrease(1);
            //else Debug.Log("GAME OVER");
        }

        public void OnTrigger(GameObject _) {}
    }
}
