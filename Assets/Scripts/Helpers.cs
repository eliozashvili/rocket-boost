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
        ReloadScene,
        DisableCollision
    }
    /// <summary>
    /// Handles scene loading based on specified action
    /// </summary>
    /// <param name="action">enum type scene navigation action</param>
    /// <param name="playerRef">parameter of type GameObject</param>
    public static void HandleScene(SceneActions action, GameObject playerRef = null)
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
            case SceneActions.DisableCollision:
                DisableCollisionAndInertia(player: playerRef);
                break;
            case SceneActions.None:
                default:
                    break;
        }
    }
    /// <summary>
    /// DEBUG: Handles scene change on key press (0, -, =)
    /// <param name="playerRef">parameter of type GameObject</param>
    /// </summary>
    public static void RespondToDebugKeys(GameObject playerRef)
    {
        if (Keyboard.current.equalsKey.wasPressedThisFrame)
            HandleScene(action: SceneActions.LoadNextScene);
        else if (Keyboard.current.minusKey.wasPressedThisFrame)
            HandleScene(action: SceneActions.LoadPreviousScene);
        else if (Keyboard.current.digit0Key.wasPressedThisFrame)
            HandleScene(action: SceneActions.ReloadScene);
        else if (Keyboard.current.digit9Key.wasPressedThisFrame)
            HandleScene(action: SceneActions.DisableCollision, playerRef: playerRef);
    }
    /// <summary>
    /// DEBUG: disables gravity and collision (9)
    /// <param name="player">parameter of type GameObject</param>
    /// </summary>
    private static void DisableCollisionAndInertia(GameObject player)
    {
        if (!player.CompareTag("Player")) return;

        player.TryGetComponent<Collider>(out var collider);
        player.TryGetComponent<Rigidbody>(out var rb);
        collider.enabled = false;
        rb.useGravity = false;
        rb.linearDamping = 5;
    }
}
