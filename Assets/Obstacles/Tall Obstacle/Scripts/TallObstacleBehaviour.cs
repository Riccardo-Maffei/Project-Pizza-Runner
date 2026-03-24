using UnityEngine;

using utils;


namespace Obstacles.Tall_Obstacle.Scripts
{
    public class TallObstacleBehaviour : MonoBehaviour, IInteractive
    {
        public void OnCollision(GameObject _)
        {
            // 1. Check if damage is enabled (prevents double damage)
            if (!GameData.DamageEnabled.GetValue()) return;

            // Briefly block damage
            GameData.DamageEnabled.SetValue(false);

            if (GameData.Hp.GetValue() > 0)
            {
                GameData.Hp.Decrease(1);
                Debug.Log("HP Reduced! HP Left: " + GameData.Hp.GetValue());

                // Turn damage back on after half a second
                Invoke(nameof(ResetDamage), 0.5f);
            }

            // 3. Check if the game has been lost
            if (GameData.Hp.GetValue() <= 0)
            {
                EndGame();
            }
        }

        // Helper method to reenable damage
        private void ResetDamage()
        {
            GameData.DamageEnabled.SetValue(true);
        }

        private static void EndGame()
        {
            Debug.Log("GAME OVER - Lost all lives.");

            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #else
                Application.Quit();
            #endif
        }

        public void OnTrigger(GameObject _) {}
    }
}
