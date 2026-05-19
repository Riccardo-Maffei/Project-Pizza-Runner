using UnityEngine.SceneManagement;


namespace Utils
{
    public static class GameHandler
    {
        private const int MenuSceneIndex = 0;
        private const int GameSceneIndex = 1;
        
        public const int Tutorial1SceneIndex = 2;
        public const int Tutorial2SceneIndex = 3;
        public const int Tutorial3SceneIndex = 4;
        public const int Tutorial4SceneIndex = 5;
        public const int Tutorial5SceneIndex = 6;
        
        public static void LoadMenuScene()
        {
            SceneManager.LoadScene(MenuSceneIndex);
        }
        
        public static void LoadGameScene()
        {
            SceneManager.LoadScene(GameSceneIndex);
        }
        
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
