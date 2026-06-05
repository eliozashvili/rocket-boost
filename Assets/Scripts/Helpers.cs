using UnityEngine.SceneManagement;

public static class Helpers
{
    /// <summary>
    /// Loads next scene or reloads current scene
    /// </summary>
    /// <param name="isNext">loads next scene if true</param>
    /// <param name="reload">reloads current scene if true, default is false</param>
    public static void HandleScene(bool isNext, bool reload = false)
    {
        if (!isNext && !reload) return;

        var sceneIndex = SceneManager.GetActiveScene().buildIndex;

        if (reload)
        {
            SceneManager.LoadScene(sceneIndex);
            return;
        }

        var nextSceneIndex = sceneIndex == SceneManager.sceneCountInBuildSettings - 1 ? 0 : sceneIndex + 1;

        SceneManager.LoadScene(nextSceneIndex);
    }
}
