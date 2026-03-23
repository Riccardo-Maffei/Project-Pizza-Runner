using Collectibles.Scripts;

using utils;


namespace Collectibles.Coin
{
    public class CoinBehaviour : CollectibleBase
    {
        public int coinValue = 10;

        protected override void TriggerBehaviour()
        {
            GameData.Coins.Increase(coinValue);
        }
    }
}
