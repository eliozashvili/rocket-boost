using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public static class Helpers
{
    public enum SceneActions
    {
        None,
        LoadNextScene,
        LoadPreviousScene,
        ReloadScene
    }
    /// <summary>
    /// Handles scene loading based on specified action
    /// </summary>
    /// <param name="action">enum type scene navigation action</param>
    public static void HandleScene(SceneActions action)
    {
        var sceneIndex = SceneManager.GetActiveScene().buildIndex;

        switch (action)
        {
            case SceneActions.LoadNextScene:
                var nextSceneIndex = sceneIndex == SceneManager.sceneCountInBuildSettings - 1 ? 0 : sceneIndex + 1;
                SceneManager.LoadScene(nextSceneIndex);
                break;
            case SceneActions.LoadPreviousScene:
                var previousSceneIndex = sceneIndex != 0 ? sceneIndex - 1 : 0;
                SceneManager.LoadScene(previousSceneIndex);
                break;
            case SceneActions.ReloadScene:
                SceneManager.LoadScene(sceneIndex);
                break;
            case SceneActions.None:
                default:
                    break;
        }
    }
    /// <summary>
    /// DEBUG: Handles scene change on key press (0, -, =)
    /// </summary>
    public static void RespondToDebugKeys()
    {
        if (Keyboard.current.equalsKey.wasPressedThisFrame)
            HandleScene(action: SceneActions.LoadNextScene);
        else if (Keyboard.current.minusKey.wasPressedThisFrame)
            HandleScene(action: SceneActions.LoadPreviousScene);
        else if (Keyboard.current.digit0Key.wasPressedThisFrame)
            HandleScene(action: SceneActions.ReloadScene);
    }
}
