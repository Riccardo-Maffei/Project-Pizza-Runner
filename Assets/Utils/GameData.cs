namespace utils
{
    public static class GameData
    {
        private const int StartingHp = 2;

        public static readonly Observable<float> CurrentTime = new(0);
        public static readonly Observable<double> TotalDistance = new(0);

        public static readonly Observable<int> Coins = new(0);
        public static readonly Observable<int> Pizzas = new(0);
        public static readonly Observable<int> Hp = new(StartingHp);

        public static void Reset()
        {
            CurrentTime.SetValue(0);
            TotalDistance.SetValue(0);
            Coins.SetValue(0);
            Pizzas.SetValue(0);
            Hp.SetValue(StartingHp);
        }
    }
}
