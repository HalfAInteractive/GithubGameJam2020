using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace Drw
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(HealthComponent))]
    public class DrwPlayerController : MonoBehaviour//, IPlayer
    {
        [Header("Player Settings")]
        [Tooltip("The amount of fwd (forward/backward) force applied by the player.")]
        [SerializeField] 
        float forwardForceAmount = 20f;

        [Tooltip("The amount of turn (left/right) force applied by the player.")]
        [SerializeField]
        float turnForceAmount = 20f;

        [Tooltip("The amount of vertical(up/down) force applied by player")]
        [SerializeField]
        float verticalForceAmount = 20f;

        [Tooltip("The max amount of speed the player can go.")]
        [SerializeField] 
        float maxSpeed = 25f;

        [Space(5)]
        [Header("Trail settings")]
        [Tooltip("Speed for when the trails will activate. If it goes less than the speed, it stops.")]
        [SerializeField] 
        [Range(0, 50)]
        float trailSpeedActive = 4f;
        
        [SerializeField] TrailRenderer trailLeft = null;
        [SerializeField] TrailRenderer trailRight = null;
        [SerializeField] GameObject explosionTemp = null;
        [SerializeField] AudioClip hurtSFX = null;

        HealthComponent healthComp = null;

        Rigidbody playerRigidbody = null;
        InputControls inputControls = null;
        Vector2 inputVal;
        Camera cam = null;
        float currSpeed = 0f;
        AudioSource audioSource = null;
        bool isMovingUpPressed = false, isMovingDownPressed = false;

        public event Action OnPlayerKilled = delegate { };
        public event Action OnPlayerDamaged = delegate { };

        private void OnValidate()
        {
            forwardForceAmount = forwardForceAmount < 0f ? 0f : forwardForceAmount;
            maxSpeed = maxSpeed < 0f ? 0f : maxSpeed;
        }

        private void Awake()
        {
            playerRigidbody = GetComponent<Rigidbody>();
            cam = FindObjectOfType<Camera>();

            inputControls = new InputControls();
            healthComp = GetComponent<HealthComponent>();
            audioSource = GetComponent<AudioSource>();
        }

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            trailLeft.emitting = false;
            trailRight.emitting = false;
        }

        private void OnEnable()
        {
            inputControls.Player.Move.performed += ctx => inputVal = ctx.ReadValue<Vector2>();
            inputControls.Player.Move.canceled += ctx => inputVal = Vector3.zero;

            inputControls.Player.Jump.performed += ctx => isMovingUpPressed = true;
            inputControls.Player.Jump.canceled += ctx => isMovingUpPressed = false;

            inputControls.Player.Crouch.performed += ctx => isMovingDownPressed = true;
            inputControls.Player.Crouch.canceled += ctx => isMovingDownPressed = false; 

            inputControls.Enable();
        }
        private void OnDisable()
        {
            inputControls.Player.Move.performed -= ctx => inputVal = ctx.ReadValue<Vector2>();
            inputControls.Player.Move.canceled -= ctx => inputVal = Vector3.zero;

            inputControls.Player.Jump.performed -= ctx => isMovingUpPressed = true;
            inputControls.Player.Jump.canceled -= ctx => isMovingUpPressed = false;

            inputControls.Player.Crouch.performed -= ctx => isMovingDownPressed = true;
            inputControls.Player.Crouch.canceled -= ctx => isMovingDownPressed = false;

            inputControls.Disable();
        }

        private void Update()
        {
            bool isUsingInput = inputVal.sqrMagnitude > 0f;
            bool enableSpeedTrails = currSpeed > trailSpeedActive;

            trailLeft.emitting = isUsingInput || enableSpeedTrails;
            trailRight.emitting = isUsingInput || enableSpeedTrails;
        }

        private void FixedUpdate()
        {
            currSpeed = playerRigidbody.velocity.magnitude;
            MovePlayer();
        }

        void MovePlayer()
        {
            if (IsAtMaxSpeed) return;

            // TODO - will have to switch depending on the cam mode
            Vector3 newFwd = cam.transform.forward;
            Vector3 newRight = Vector3.Cross(cam.transform.up, newFwd);

            Vector3 verticalForce = Vector3.zero;
            if(isMovingUpPressed && !isMovingDownPressed)
            {
                verticalForce = transform.up * verticalForceAmount;
            }
            else if(isMovingDownPressed && !isMovingUpPressed)
            {
                verticalForce = transform.up * (-1f * verticalForceAmount);
            }

            Vector3 dir = newFwd * (inputVal.y * forwardForceAmount) + newRight * (inputVal.x * turnForceAmount) + verticalForce;

            playerRigidbody.AddForce(dir);
        }

        public void Boost(float amount)
        {
            Debug.Log($"Boosting player for : {amount}", this);
            
            Vector3 dir = playerRigidbody.velocity.normalized;
            playerRigidbody.AddForce(dir * amount);
        }

        public void TakeDamage(int amount)
        {
            if (healthComp.IsDead) return; // already dead so don't run thingsbelow

            Debug.Log($"Taking damage for : {amount}");

            healthComp.TakeDamage(amount);
            if(healthComp.IsDead)
            {
                Kill();
            }
            else
            {
                OnPlayerDamaged();
                audioSource.PlayOneShot(hurtSFX);
            }
        }
        
        public void Kill()
        {
            OnPlayerKilled();

            // temp
            Instantiate(explosionTemp, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }

        public void Heal(int amount)
        {
            healthComp.Heal(amount);
        }

        bool IsAtMaxSpeed => currSpeed > maxSpeed;
    }
}