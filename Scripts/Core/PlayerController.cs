using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
using DG.Tweening;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(HealthComponent))]
public class PlayerController : MonoBehaviour, IPlayer
{
    #region serialize fields
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

    [Tooltip("The amount of power the player can exert onto attached ammo. Must be 1 or greater. Ammo's mass is taken into account when shooting.")]
    [SerializeField]
    float shootingPower = 80f;

    [SerializeField]
    Camera cam = null;

    //[SerializeField]
    //InputActionReference inputRef;

    [Space(5)]
    [Header("Trail settings")]
    [Tooltip("Speed for when the trails will activate. If it goes less than the speed, it stops.")]
    [SerializeField]
    [Range(0, 50)]
    float trailSpeedActive = 4f;

    [SerializeField] TrailRenderer trailLeft = null;
    [SerializeField] TrailRenderer trailRight = null;
    [SerializeField] GameObject explosionTemp = null;
    [SerializeField] ParticleSystem fogEmitter;

    [Space(5)]
    [Header("Audio")]
    [SerializeField] AudioClip ammoLoad = null;


    #endregion

    #region private fields
    Queue<IAmmo> ammunition;
    
    HealthComponent healthComp = null;
    Rigidbody playerRigidbody = null;
    InputControls inputControls = null;
    AudioSource audioSource = null;

    public int StarCoins { get; private set; } = 0;
    float currSpeed = 0f;
    bool isMovingUpPressed = false, isMovingDownPressed = false;
    
    Vector2 inputVal = Vector2.zero;
    Vector3 midpoint = Vector3.one * 0.5f;
    RaycastHit camHitInfo;
    Vector3 aimPos = Vector3.zero;
    float aimDist = 100;

    IFocusable focusedObject;
    const float MAX_RAYCAST_LEN = 2000f;
    Ray camRay;

    int playerLayerMask = 1 << GameConstants.PLAYER_LAYER;

    public bool IsAlive => !healthComp.IsDead;
    #endregion

    public Camera PlayerCamera => cam;
    public int CurrentAmmoCount => ammunition.Count;
    public bool IsAmmoEmpty => ammunition.Count == 0;

    public event Action OnPlayerKilled = delegate { };
    public event Action<int> OnPlayerDamaged = delegate { };
    public event Action<int> OnPlayerStarCollect = delegate { };

    private void OnValidate()
    {
        forwardForceAmount = forwardForceAmount < 0f ? 0f : forwardForceAmount;
        maxSpeed = maxSpeed < 0f ? 0f : maxSpeed;
        shootingPower = shootingPower < 1f ? 1f : shootingPower;
    }

    private void Awake()
    {
        inputControls = new InputControls();
        playerRigidbody = GetComponent<Rigidbody>();
        healthComp = GetComponent<HealthComponent>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        trailLeft.emitting = false;
        trailRight.emitting = false;
        
        ammunition = new Queue<IAmmo>();
        playerLayerMask = ~playerLayerMask;
    }

    private void OnEnable()
    {
        inputControls.Player.Move.performed += ctx => inputVal = ctx.ReadValue<Vector2>();
        inputControls.Player.Move.canceled += ctx => inputVal = Vector3.zero;

        inputControls.Player.Jump.performed += ctx => isMovingUpPressed = true;
        inputControls.Player.Jump.canceled += ctx => isMovingUpPressed = false;

        inputControls.Player.Crouch.performed += ctx => isMovingDownPressed = true;
        inputControls.Player.Crouch.canceled += ctx => isMovingDownPressed = false;
        
        inputControls.Player.Fire.performed += ctx => Fire();
        
        inputControls.Player.Scan.performed += ctx => Scan();
        inputControls.Player.Scan.canceled += ctx => ScanRelease();

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
        
        inputControls.Player.Fire.performed -= ctx => Fire();
        
        inputControls.Player.Scan.performed -= ctx => Scan();
        inputControls.Player.Scan.canceled -= ctx => ScanRelease();

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
        RaycastFindFocusableObjects();
    }

    private void RaycastFindFocusableObjects()
    {
        camRay = cam.ViewportPointToRay(midpoint);

        aimDist = 200f;

        if (Physics.Raycast(camRay, out camHitInfo, maxDistance: 150f, playerLayerMask))
        {
            if (camHitInfo.transform.TryGetComponent(out IFocusable currentFocus))
            {
                if (focusedObject != null && focusedObject != currentFocus)
                {
                    focusedObject.OutOfLineOfSights();
                }// case when we have 2 objects next to each other with no gap.

                focusedObject = currentFocus;
                focusedObject.InLineOfSights();
            }
            else if (focusedObject != null)
            {
                focusedObject.OutOfLineOfSights();
                focusedObject = null;
            }
#if UNITY_EDITOR
            //print($"launching : {gameObject} at {Time.realtimeSinceStartup}");
            Debug.DrawRay(transform.position, (camHitInfo.transform.position - transform.position), Color.red, .5f);
#endif
            aimDist = (camHitInfo.transform.TryGetComponent(out ITarget aim)) ? (camHitInfo.transform.position - transform.position).magnitude : 200f;
            aimPos = (camHitInfo.transform.TryGetComponent(out IAutoTarget aTarget)) ? camHitInfo.transform.position : Vector3.zero;
// case if there are colliders in general in the background within raycast distance
        }
        else if (focusedObject != null)
        {
            focusedObject.OutOfLineOfSights();
            focusedObject = null;
        }// case if didn't find any colliders
        else
        {
            // fixes bug where if this is assigned from above for the auto aim
            // it will reset after the raycast fails. without it, if you were to hit a target
            // then shoot to the side in an empty void, it will go back to that auto aim target.
            aimPos = Vector3.zero; 
        }
    }

    void MovePlayer()
    {
        if (IsAtMaxSpeed) return;

        // TODO - will have to switch depending on the cam mode
        Vector3 newFwd = cam.transform.forward;
        Vector3 newRight = Vector3.Cross(cam.transform.up, newFwd);

        Vector3 verticalForce = Vector3.zero;
        if (isMovingUpPressed && !isMovingDownPressed)
        {
            verticalForce = transform.up * verticalForceAmount;
        }
        else if (isMovingDownPressed && !isMovingUpPressed)
        {
            verticalForce = transform.up * (-1f * verticalForceAmount);
        }

        Vector3 dir = newFwd * (inputVal.y * forwardForceAmount) + newRight * (inputVal.x * turnForceAmount) + verticalForce;

        playerRigidbody.AddForce(dir);
    }

    public void Boost(float amount, ForceMode forceMode)
    {
#if UNITY_EDITOR
        Debug.Log($"Boosting player for : {amount}", this);
#endif
        Vector3 dir = playerRigidbody.velocity.normalized;
        playerRigidbody.AddForce(dir * amount, forceMode);
    }

    /// <summary>
    /// Used more like a bumper effect. You can get the player.position - target.position and it will bounce you that way.
    /// </summary>
    /// <param name="amount"></param>
    /// <param name="dir"></param>
    /// <param name="forceMode"></param>
    public void Bounce(float amount, Vector3 targetPos, ForceMode forceMode)
    {
#if UNITY_EDITOR
        Debug.Log($"Boosting player for : {amount}", this);
#endif
        Vector3 dir = (transform.localPosition - targetPos).normalized;
        playerRigidbody.AddForce(dir * amount, forceMode);
    }
    
    public void Push(Vector3 dir)
    {
        playerRigidbody.AddForce(dir);
    }

    public void TakeDamage(int amount)
    {
        if (healthComp.IsDead) return; // already dead so don't run thingsbelow

        //Debug.Log($"Taking damage for : {amount}");

        if (!GetComponent<StarCollector>().HasRing)
        {
            healthComp.TakeDamage(amount);
        }

        if (healthComp.IsDead)
        {
            Kill();
        }
        else
        {
            OnPlayerDamaged(healthComp.CurrHealth);
        }
    }
    
    public void CollectCoin(int amt)
    {
        OnPlayerStarCollect(amt);

        StarCoins += amt;
        //Debug.Log("Deposited: " + amt);
        //Debug.Log("Total Coins: " + coins);
    }

    public void Kill()
    {
        OnPlayerKilled();

        // temp
        gameObject.SetActive(false);
        //Destroy(gameObject);
    }

    public void Heal(int amount)
    {
        healthComp.Heal(amount);
    }
    
    public void Fire()
    {
        //Debug.Log($"FIRE! Curr ammo: {ammunition.Count}");
        if(ammunition.Count > 0)
        {
            IAmmo ammo = ammunition.Dequeue();

            if (aimPos != Vector3.zero)
            {
                ammo.Fire(aimPos, 80, 0);
            }
            else
            {
                ammo.Fire(camRay.direction, 80, aimDist);
            }
            
            if (ammunition.Count > 0)
                ammunition.Peek().Select();
        }
    }
    
    public void Scan()
    {
        fogEmitter.Play();

        //var emission = fogEmitter.emission;
        //emission.rateOverTime = 50;
    }
    
    public void ScanRelease()
    {
        fogEmitter.Stop();
        //var emission = fogEmitter.emission;
        //emission.rateOverTime = 0;
        //Debug.Log("RELEASED");
    }
    
    public void Reload(IAmmo ammo)
    {
#if UNITY_EDITOR
        print($"loading in : {ammo}");
#endif
        ammunition.Enqueue(ammo);
        audioSource.PlayOneShot(ammoLoad);
        ammunition.Peek().Select();
    }

    bool IsAtMaxSpeed => currSpeed > maxSpeed;

    public void DisableControls() => inputControls.Disable();

    public void EnableControls() => inputControls.Enable();

    public Vector3 GetPosition() => transform.localPosition;
}