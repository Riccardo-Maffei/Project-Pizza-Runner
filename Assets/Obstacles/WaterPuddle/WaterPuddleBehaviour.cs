using Utils;
using UnityEngine;


namespace Obstacles.WaterPuddle
{
    public class WaterPuddleBehaviour : MonoBehaviour, IInteractive
    {
        public float playerSpeedMultiplier = 0.4f;
        public int multiplierDuration = 2;
        
        public AudioClip slippingSound;
        
        public void OnTrigger(GameObject _)
        {
            GameData.SpeedMultipliers.Add(playerSpeedMultiplier);
            GameData.IsSlipping.SetValue(true);
            
            AudioSource.PlayClipAtPoint(slippingSound, transform.position);
            
            Delay.BySeconds(RemoveSlowdown, multiplierDuration);
        }

        private void RemoveSlowdown()
        {
            GameData.SpeedMultipliers.Remove(playerSpeedMultiplier);
            GameData.IsSlipping.SetValue(false);
        }
    }
}
