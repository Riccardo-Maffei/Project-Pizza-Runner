namespace utils
{
    public static class GameData
    {
        // TODO: Update time with time.deltaTime (UI Class)
        public static float CurrentTime = 0;
        public static float TotalDistance = 0;

        public static int Coins = 0;
        public static int Pizzas = 0;
        public static int Hp = 1;

        // TODO: Create UI class and add reset values there
        public static void Reset()
        {
            CurrentTime = 0;
            TotalDistance = 0;
            Coins = 0;
            Pizzas = 0;
            Hp = 1;
        }
    }
}
