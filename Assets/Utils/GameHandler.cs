using UnityEngine.SceneManagement;


namespace Utils
{
    public static class GameHandler
    {
        const int MenuSceneIndex = 0;
        const int GameSceneIndex = 1;
        
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
