using UnityEngine;
using Utils;


namespace Obstacles.WaterPuddle
{
    public class WaterPuddleBehaviour : MonoBehaviour, IInteractive
    {
        public float playerSpeedMultiplier = 0.4f;
        public int multiplierDuration = 2;
        
        public void OnTrigger(GameObject _)
        {
            GameData.SpeedMultipliers.Add(playerSpeedMultiplier);
            GameData.IsSlipping.SetValue(true);
            
            Delay.BySeconds(RemoveSlowdown, multiplierDuration);
        }

        private void RemoveSlowdown()
        {
            GameData.SpeedMultipliers.Remove(playerSpeedMultiplier);
            GameData.IsSlipping.SetValue(false);
        }
    }
}
