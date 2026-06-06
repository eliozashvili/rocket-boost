using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Movement))]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Rigidbody))]
public class CollisionHandler : MonoBehaviour
{
    [SerializeField] private AudioClip crashSfx;
    [SerializeField] private AudioClip successSfx;
    [SerializeField] private ParticleSystem crashParticle;
    [SerializeField] private ParticleSystem successParticle;

    private readonly WaitForSeconds _delay = new (2f);
    private Movement _movementScript;
    private AudioSource _audioSource;
    private Rigidbody _rb;

    private void Start()
    {
        _movementScript = GetComponent<Movement>();
        _audioSource =  GetComponent<AudioSource>();
        _rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Helpers.RespondToDebugKeys();
    }

    private void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                break;
            case "Finish":
                StartCoroutine(HandleSceneAfterDelay(Helpers.SceneActions.LoadNextScene, audioClip: successSfx));
                successParticle.Play();
                break;
            default:
                StartCoroutine(HandleSceneAfterDelay(Helpers.SceneActions.ReloadScene, audioClip: crashSfx));
                crashParticle.Play();
                break;
        }
    }

    private IEnumerator HandleSceneAfterDelay(Helpers.SceneActions action, AudioClip audioClip)
    {
        // disabling Movement.cs forces to execute OnDisable method
        _movementScript.enabled = false;
        _rb.isKinematic = true;

        if (audioClip)
            _audioSource.PlayOneShot(audioClip);

        yield return _delay;
        Helpers.HandleScene(action);
    }
}
