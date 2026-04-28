using Player.Scripts;
using UnityEngine;
using Utils;

namespace Obstacles.Tall_Obstacle.Scripts
{
    public class TallObstacleBehaviour : MonoBehaviour, IInteractive
    {
        [SerializeField] private GameObject hitParticlesPrefab;
        public void OnCollision(GameObject _)
        {
            // 1. Check if damage is enabled (prevents double damage)
            if (!GameData.DamageEnabled.GetValue()) return;

            if (hitParticlesPrefab != null)
            {
               Instantiate(hitParticlesPrefab, transform.position, Quaternion.identity);
            }
            Destroy(gameObject);
            
            // Briefly block damage
            GameData.DamageEnabled.SetValue(false);

            if (GameData.Hp.GetValue() > 0)
            {
                GameData.Hp.Decrease(1);
                Debug.Log("HP Reduced! HP Left: " + GameData.Hp.GetValue());

                // Turn damage back on after half a second
                Delay.BySeconds(ResetDamage, 0.5f);
            }
        }

        private static void ResetDamage()
        {
            GameData.DamageEnabled.SetValue(true);
        }
    }
}