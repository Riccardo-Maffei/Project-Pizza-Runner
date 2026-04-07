using Collectibles.Scripts;

using Utils;


namespace Collectibles.Coffee
{
    public class CoffeeBehaviour : CollectibleBase
    {
        public float timeLockDuration = 5;

        protected override void TriggerBehaviour()
        {
            if (GameData.ReversedCommands.GetValue()) GameData.ReversedCommands.SetValue(false);
            else GameData.TimeLock.Increase(timeLockDuration);
        }
    }
}
