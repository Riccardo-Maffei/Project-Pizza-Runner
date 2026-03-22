using UnityEngine;

using utils;
using Player.Scripts;


namespace Track.Finish_Line.Scripts
{
    public class FinishLineBehaviour : MonoBehaviour, IInteractive
    {
        public void OnCollision(GameObject _) {}

        public void OnTrigger(GameObject player) {
            GameData.CrossedFinishLine.SetValue(true);

            var playerBehaviour = player.GetComponent<PlayerBehaviour>();
            if (playerBehaviour != null) playerBehaviour.maxPlayerSpeed = 0;

        }
    }
}
