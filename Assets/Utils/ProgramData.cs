namespace Utils
{
    public static class ProgramData
    {
        public static readonly Observable<int> TotalCoins = new(0);
        public static readonly Observable<int> NewGameTrackLength = new(200);
    }
}
