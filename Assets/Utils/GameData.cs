using System.Collections.Generic;

namespace Utils
{
    public static class GameData
    {
        private const int StartingHp = 2;

        public static readonly Observable<float> CurrentTime = new(0);
        public static readonly Observable<float> TimeLock = new(0);
        
        public static readonly List<float> SpeedMultipliers = new ();

        public static readonly Observable<double> TotalDistance = new(0);

        public static readonly Observable<int> Coins = new(0);
        public static readonly Observable<int> Pizzas = new(0);
        public static readonly Observable<int> Hp = new(StartingHp);

        public static readonly Observable<float> TrackLength = new(100);
        public static readonly Observable<bool> SpawnedFinishLine = new(false);
        public static readonly Observable<bool> CrossedFinishLine = new(false);

        public static readonly Observable<bool> DamageEnabled = new(true);
        public static readonly Observable<bool> ReversedCommands = new(false);
        public static readonly Observable<bool> IsSlipping = new(false);

        public static void Reset()
        {
            CurrentTime.SetValue(0);
            TotalDistance.SetValue(0);

            SpeedMultipliers.Clear();

            Coins.SetValue(0);
            Pizzas.SetValue(0);
            Hp.SetValue(StartingHp);

            TrackLength.SetValue(ProgramData.NewGameTrackLength.GetValue());
            SpawnedFinishLine.SetValue(false);
            CrossedFinishLine.SetValue(false);

            DamageEnabled.SetValue(true);
            ReversedCommands.SetValue(false);
            IsSlipping.SetValue(false);
        }
    }
}
