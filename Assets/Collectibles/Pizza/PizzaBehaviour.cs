using Collectibles.Scripts;

using utils;


namespace Collectibles.Pizza
{
    public class PizzaBehaviour : CollectibleBase
    {
        private const int PizzaValue = 1;

        protected override void TriggerBehaviour()
        {
            GameData.Pizzas.Increase(PizzaValue);
        }
    }
}
