using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Movement))]
public class CollisionHandler : MonoBehaviour
{

    private readonly WaitForSeconds _delay = new (2f);
    private Movement _movementScript;

    private void Start()
    {
        _movementScript = GetComponent<Movement>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
             case "Friendly":
                 break;
            case "Finish":
                StartCoroutine(HandleAfterDelay(isNext: true, reload: false));
                break;
            default:
                StartCoroutine(HandleAfterDelay(isNext: false, reload: true));
                break;
        }
    }

    private IEnumerator HandleAfterDelay(bool isNext, bool reload)
    {
        _movementScript.enabled = false;

        yield return _delay;
        Helpers.HandleScene(isNext, reload);
    }
}
