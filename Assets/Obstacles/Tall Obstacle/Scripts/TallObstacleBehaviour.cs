using UnityEngine;

using Utils;


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
                Delay.BySeconds(ResetDamage, 0.5f);
            }

            // 3. Check if the game has been lost
            if (GameData.Hp.GetValue() > 0) return;

            Delay.BySeconds(GameHandler.LoadMenuScene, 5);
            
        }

        // Helper method to reenable damage
        private static void ResetDamage()
        {
            GameData.DamageEnabled.SetValue(true);
        }
    }
}
