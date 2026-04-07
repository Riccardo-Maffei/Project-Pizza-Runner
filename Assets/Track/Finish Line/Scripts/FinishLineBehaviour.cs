using UnityEngine;
using Utils;
using Player.Scripts;

namespace Track.Finish_Line.Scripts
{
    public class FinishLineBehaviour : MonoBehaviour, IInteractive
    {
        public void OnCollision(GameObject _) {}

        public void OnTrigger(GameObject player) 
        {
            GameData.CrossedFinishLine.SetValue(true);

            var rb = player.GetComponent<Rigidbody2D>();
            if (rb != null) 
            {
                rb.linearVelocity = Vector2.zero;
                rb.angularVelocity = 0f;
                rb.bodyType = RigidbodyType2D.Kinematic;
            }

            var playerBehaviour = player.GetComponent<PlayerBehaviour>();
            if (playerBehaviour != null) 
            {
                playerBehaviour.maxPlayerSpeed = 0;
            }

            ProgramData.TotalCoins.Increase(GameData.Coins.GetValue());

            Delay.BySeconds(GameHandler.LoadMenuScene, 5);
        }
    }
}