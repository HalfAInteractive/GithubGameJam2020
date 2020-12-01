using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// TODO - can probably make a more generic class spawner that can be reusable.  
// NOTE : instantiating objects with more than one scene instantiates them on the other scene
//      even when the scene is marked active. when i was instantiating the moons, it would go
//      to the base scene even though the tutorial is the active one. the work around is to 
//      set the object as a child to this then unparent them. ¯\_(ツ)_/¯
public class MoonSpawner : MonoBehaviour
{
    [SerializeField] Moon moon = null;
    [SerializeField] int numOfMoons = 10;

    [Tooltip("Spawn radius where the moon spawns within on a sphere. It doesn't spawn inside of it.")]
    [SerializeField] float randomSpawnRadius = 90f;

    [SerializeField] bool initiateAtStart = false;
    [SerializeField] float startDelay = 1f;

    List<Moon> moons = null;
    bool isRunning = false;

    private void Awake()
    {
        moons = new List<Moon>();

        for(int i = 0; i < numOfMoons; i++)
        {
            Vector3 randPos = Random.onUnitSphere * randomSpawnRadius;
            var temp = Instantiate(moon, randPos, Quaternion.identity);
            temp.transform.SetParent(transform); 
            temp.gameObject.SetActive(false);
            moons.Add(temp);
        }

        transform.DetachChildren();
    }

    private void OnEnable()
    {
        if(initiateAtStart)
        {
            InitiateSpawner();
        }
    }

    public void InitiateSpawner()
    {
        if (isRunning) return;
//        if(initiateAtStart)
//        {
//#if UNITY_EDITOR
//            Debug.LogError($"ERR: {this} is already spawning moons. initiateAtStart is set to true. Did you mean to?", this);
//#endif
//            return;
//        }



        StartCoroutine(MoonSpawnerCoroutine());
    }

    IEnumerator MoonSpawnerCoroutine()
    {
        yield return new WaitForSeconds(startDelay);

        foreach (var moon in moons)
        {
            moon.gameObject.SetActive(true);
            yield return new WaitForSeconds(Random.Range(0.3f, 0.6f));
        }

        isRunning = true;
        var wait = new WaitForSeconds(1f);
        

        while (true)
        {
            // our moon currently can't be pooled so this is plan b.
            // when player launches it, it destroys itself. to be sure there's always enough moons
            // in the scene, we create another one.
            for(int i = 0; i < numOfMoons; i++)
            {
                if(moons[i] == null) // if got destroyed
                {
                    Vector3 randPos = Random.onUnitSphere * randomSpawnRadius;
                    var temp = Instantiate(moon, randPos, Quaternion.identity);
                    temp.transform.SetParent(transform);
                    transform.DetachChildren();
                    moons[i] = temp;
                }
            }

            yield return wait;
        }
    }
}
