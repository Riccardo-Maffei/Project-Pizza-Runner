using UnityEngine;


namespace Utils
{
    public static class GameHandler
    {
        public static void EndGame()
        {
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #else
                Application.Quit();
            #endif
        }
    }
}
