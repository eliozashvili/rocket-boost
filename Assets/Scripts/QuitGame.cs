using UnityEngine;
using UnityEngine.InputSystem;

public class QuitGame : MonoBehaviour
{
    [SerializeField] private InputAction quitGame;

    private void OnEnable()
    {
        quitGame.Enable();

        quitGame.performed += _ => Application.Quit();
    }

    private void OnDisable()
    {
        quitGame.Disable();
    }
}
