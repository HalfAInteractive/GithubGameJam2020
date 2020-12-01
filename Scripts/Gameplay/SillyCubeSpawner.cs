using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// I knew i should've made a spawner class....
public class SillyCubeSpawner : MonoBehaviour
{
    [SerializeField] SillyCube sillyCube;
    [SerializeField] int numOfSillyCubes = 15;

    [Tooltip("Spawn radius where the moon spawns within on a sphere. It doesn't spawn inside of it.")]
    [SerializeField] float randomSpawnRadius = 90f;

    List<SillyCube> sillyCubes;

    void Awake()
    {
        sillyCubes = new List<SillyCube>();

        for (int i = 0; i < numOfSillyCubes; i++)
        {
            Vector3 randPos = Random.onUnitSphere * randomSpawnRadius;
            var temp = Instantiate(sillyCube, randPos, Random.rotation);
            temp.transform.SetParent(transform);
            temp.gameObject.SetActive(false);
            sillyCubes.Add(temp);
        }
    }

    IEnumerator Start()
    {
        yield return new WaitForSeconds(4f);


        foreach (var cube in sillyCubes)
        {
            cube.gameObject.SetActive(true);
            yield return new WaitForSeconds(Random.Range(1f, 3f));
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, randomSpawnRadius);
    }
}
