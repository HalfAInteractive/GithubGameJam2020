using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoTargetSpawner : MonoBehaviour
{
    [SerializeField] AmmoTarget ammoTarget;
    [SerializeField] int numOfTargets = 10;

    [Tooltip("Spawn radius where the moon spawns within on a sphere. It doesn't spawn inside of it.")]
    [SerializeField] float randomSpawnRadius = 90f;

    [SerializeField] float startDelay = 1f;

    List<AmmoTarget> ammoTargets = null;

    public int NumOfConfirmedHit { get; private set; } = 0;

    private void Awake()
    {
        ammoTargets = new List<AmmoTarget>();

        for(int i = 0; i < numOfTargets; i++)
        {
            Vector3 randPos = Random.onUnitSphere * randomSpawnRadius;
            var temp = Instantiate(ammoTarget, randPos, Quaternion.identity);
            temp.transform.SetParent(transform);
            temp.gameObject.SetActive(false);
            ammoTargets.Add(temp);

            temp.OnAmmoEntered += CountConfirmedHit;
        }
    }

    IEnumerator Start()
    {
        yield return new WaitForSeconds(startDelay);

        var waitCheck = new WaitForSeconds(2f);

        foreach (var target in ammoTargets)
        {
            target.gameObject.SetActive(true);
            yield return new WaitForSeconds(Random.Range(.2f, .6f));
        }

        while(true)
        {
            var target = GetAmmoTarget();
            if(target != null)
            {
                target.transform.position = Random.onUnitSphere * randomSpawnRadius;
                target.gameObject.SetActive(true);
            }
            yield return waitCheck;
        }
    }

    void OnDestroy()
    {
        foreach(var target in ammoTargets)
        {
            ammoTarget.OnAmmoEntered -= CountConfirmedHit;
        }
    }

    AmmoTarget GetAmmoTarget()
    { 
        foreach(var target in ammoTargets)
        {
            if (!target.gameObject.activeSelf) return target;
        }

        return null;
    }

    void CountConfirmedHit()
    {
        NumOfConfirmedHit++;
#if UNITY_EDITOR
        Debug.Log($"Player hit target. Currrent confirmed hit : {NumOfConfirmedHit}", this);
#endif
    }
}
