using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour, IEnemy
{
    [SerializeField] bool trackPlayer = false;
    [SerializeField] bool followPlayer = false;

    [SerializeField] float moveSpeed = 10;
    [SerializeField] float firePower = 80f;

    [SerializeField] float fireRate = 10.0f;
    [SerializeField] float aggroTrackingDistance = 100f;
    [SerializeField] float aggroFollowDistance = 100f;
    [SerializeField] float aggroShootingDistance = 100f;
    
    [SerializeField] EnemyProjectile projectile = null;
    
    public bool engaged = false;
    Vector3 dir;
    float dist = -1f;

    PlayerController player;
    public bool IsAlive { get; private set; } = true;

    public PlayerController PlayerNotUsed { get; set; } = null; // this gets rid of the error in enemy aggro but is not needed

    //Timer shootTimer;
    //int count = 0;
    List<EnemyProjectile> projectiles = null;

    float timeLastFired = 0;

    AudioSource audioSource = null;

    bool initialLook = false;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    IEnumerator Start()
    {
        enabled = false; // disabling update until we get the player reference
        timeLastFired = Time.time - fireRate; // initialize time so it fires when within range instantly

        // pool 10 projectiles
        projectiles = new List<EnemyProjectile>();
        for(int i = 0; i < 10; i++)
        {
            var temp = Instantiate(projectile, transform.position, Quaternion.identity);
            temp.gameObject.SetActive(false);
            projectiles.Add(temp);
            yield return null;
        }

        
        yield return null;
        player = HelperFunctions.FindGameManagerInBaseScene().PlayerController;

        //if (shoot)
        //{
        //    shootTimer = new Timer(fireRate, -1, null, Shoot);
        //}

        enabled = true;
    }

    //void Shoot()
    //{
    //    var ammo = Instantiate(projectile, transform.position, transform.rotation);
    //    ammo.Fire(player.GetPosition() - transform.position, 0, 0);

    //    shootTimer.Reset();
    //    count++;
    //}

    EnemyProjectile GetProjectileFromPool()
    {
        foreach(var projectile in projectiles)
        {
            if (!projectile.isActiveAndEnabled) return projectile;
        }

        // add one more
        var temp = Instantiate(projectile, transform.position, Quaternion.identity);
        temp.gameObject.SetActive(false);
        projectiles.Add(temp);

        return temp;
    }

    // Update is called once per frame
    void Update()
    {
        if (!player.IsAlive) return;

        dir = (player.GetPosition() - transform.position);
        dist = dir.magnitude;

        if (trackPlayer && HelperFunctions.IsWithinInclusive(dist, 0f, aggroTrackingDistance))
        {
            transform.LookAt(player.GetPosition());

            if(!initialLook)
            {
                audioSource.Play();
                initialLook = true;
            }
        }
        else
        {
            initialLook = false;
        }

        if (followPlayer && HelperFunctions.IsWithinInclusive(dist, 0f, aggroFollowDistance))
        {
            dir.Normalize();
            transform.position += dir * (Time.deltaTime * moveSpeed);
        }

        if (HelperFunctions.IsWithinInclusive(dist, 0f, aggroShootingDistance))
        {
            if (Time.time - timeLastFired > fireRate)
            {
                var ammo = GetProjectileFromPool();
                ammo.gameObject.SetActive(true);
                ammo.transform.SetPositionAndRotation(transform.position, transform.rotation);
                ammo.Fire(dir.normalized, firePower, 0);
                timeLastFired = Time.time;
            }
        }
    }

    public void Boost(float amount, ForceMode forceMode) { }

    public void Bounce(float amount, Vector3 targetPos, ForceMode forceMode) { }

    public void TakeDamage(int amount) 
    { 
        //shootTimer.Remove(); 
        gameObject.SetActive(false);
        IsAlive = false;
    } //transform.gameObject.SetActive(false); }

    private void OnDrawGizmos()
    {
        Color yellow = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, aggroShootingDistance);
    }
}
