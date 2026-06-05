using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Movement))]
[RequireComponent(typeof(AudioSource))]
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

    private void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                break;
            case "Finish":
                StartCoroutine(HandleAfterDelay(isNext: true, reload: false, audioClip: successSfx));
                successParticle.Play();
                break;
            default:
                StartCoroutine(HandleAfterDelay(isNext: false, reload: true, audioClip: crashSfx));
                crashParticle.Play();
                break;
        }
    }

    private IEnumerator HandleAfterDelay(bool isNext, bool reload, AudioClip audioClip)
    {
        // disabling Movement.cs forces to execute OnDisable method
        _movementScript.enabled = false;
        _rb.isKinematic = true;

        if (audioClip)
            _audioSource.PlayOneShot(audioClip);

        yield return _delay;
        Helpers.HandleScene(isNext, reload);
    }
}
