using UnityEngine;
using Utils;


namespace Player.Scripts
{
    public class PlayerAnimatorBehaviour : MonoBehaviour
    {
        public string isDrunkLabel = "IsDrunk";
        public string isSlippingLabel = "IsSlipping";
        public string lowHpLabel = "LowHp";
        
        public Animator animator;
        
        private void Start()
        {
            GameData.ReversedCommands.Subscribe(DrunknessObserver);
            GameData.IsSlipping.Subscribe(SlippingObserver);
            GameData.Hp.Subscribe(LowHpObserver);
        }

        private void OnDestroy()
        {
            GameData.ReversedCommands.Unsubscribe(DrunknessObserver);
            GameData.IsSlipping.Unsubscribe(SlippingObserver);
            GameData.Hp.Unsubscribe(LowHpObserver);
        }

        private void DrunknessObserver (bool isDrunk) => animator.SetBool(isDrunkLabel, isDrunk);
        private void SlippingObserver (bool isSlipping) => animator.SetBool(isSlippingLabel, isSlipping);

        private void LowHpObserver(int hp)
        {
            if (hp == 1) animator.SetBool(lowHpLabel, true);
        } 
    }
}
