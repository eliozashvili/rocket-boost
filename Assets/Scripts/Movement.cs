using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(AudioSource))]
public class Movement : MonoBehaviour
{
   [SerializeField] private InputAction thrust;
   [SerializeField] private InputAction rotation;
   [SerializeField] private float thrustStrength;
   [SerializeField] private float rotationStrength;
   [SerializeField] private AudioClip engineThrustSfx;

   private Rigidbody _rb;
   private AudioSource _audioSource;

   private bool _isThrusting;
   private float _rotationInputValue;

   private void Start()
   {
      _rb = GetComponent<Rigidbody>();
      _audioSource = GetComponent<AudioSource>();
   }

   private void OnEnable()
   {
      thrust.Enable();
      rotation.Enable();

      thrust.performed += _ =>
      {
         _isThrusting = true;
         if (!_audioSource.isPlaying) _audioSource.PlayOneShot(engineThrustSfx);
      };

      thrust.canceled += _ =>
      {
         _isThrusting = false;
         _audioSource.Stop();
      };

      rotation.performed += context => _rotationInputValue = context.ReadValue<float>();
      rotation.canceled += _ =>  _rotationInputValue = 0f;
   }

   private void OnDisable()
   {
      thrust.Disable();
      rotation.Disable();

      _isThrusting = false;
      _rotationInputValue = 0f;

      _audioSource?.Stop();
   }

   private void FixedUpdate()
   {
      if (!_isThrusting && Mathf.Approximately(_rotationInputValue, 0f)) return;

      ProcessThrust();
      ProcessRotation();
   }

   private void ProcessThrust()
   {
      if (!_isThrusting) return;

      _rb.AddRelativeForce(thrustStrength * Time.fixedDeltaTime * Vector3.up);
   }

   private void ProcessRotation()
   {
      if (Mathf.Approximately(_rotationInputValue, 0f)) return;

      var angleZ = -_rotationInputValue * rotationStrength * Time.fixedDeltaTime;

      ApplyRotation(angleZ);
   }

   private void ApplyRotation(float angle)
   {
      var deltaRotate = Quaternion.Euler(new Vector3(0f, 0f, angle));

      _rb.MoveRotation(_rb.rotation * deltaRotate);
   }
}
