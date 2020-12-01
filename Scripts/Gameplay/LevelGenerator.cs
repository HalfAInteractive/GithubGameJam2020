using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HumanRandom))]
public class LevelGenerator : MonoBehaviour
{
    [SerializeField] List<GameObject> pieces;
    [SerializeField] public int maxRange = 1000;
    [SerializeField] int initialRange = 300;
    [SerializeField] int maxInterest = 4;
    
    List<bool> zones; // There are 10 directions in 3d space to generate objects. We don't want them to be too close to each other. Might as well have one object max per zone.
    List<Vector3> zonePositions;
    
    HumanRandom random;
    [SerializeField] public GameObject player;
    public int count = 0;
    int minimumInterest = 2;
    // Start is called before the first frame update
    void Start()
    {
        random = GetComponent<HumanRandom>();
        random.SetRange(0, pieces.Count);
        
        zones = new List<bool>{true, true, true, true, true, true};
        zonePositions = new List<Vector3>{Vector3.forward, Vector3.back, Vector3.left, Vector3.right, Vector3.up, Vector3.down};
    }
    
    void Update()
    {
        if(count < minimumInterest)
        {
            Generate();
        }
    }

    private void Generate()
    {
        while(count < maxInterest)
        {
            Quaternion randRotation = Quaternion.Euler(Random.Range(0.0f, 360.0f), Random.Range(0.0f, 360.0f), Random.Range(0.0f, 360.0f));
            int zoneNum = GetEmptyZone();
            GameObject g = GameObject.Instantiate(pieces[random.GetNextRand()], player.transform.position + zonePositions[zoneNum] * initialRange, randRotation);
            
            if(g.TryGetComponent(out PointOfInterest poi))
            {
                poi.index = zoneNum;
                poi.generator = this;
            }
            
            count++;
        }
        FloatingOrigin.Rebase(player.transform.position); // just in case rebase on the players current position. They may be going rogue.
    }
    
    private int GetEmptyZone()
    {
        while(true)
        {
            int temp = Random.Range(0, 11);
            if(zones[temp])
            {
                zones[temp] = false;
                return temp;
            }
        }
    }
    
    public void FreeZone(int index)
    {
        zones[index] = true;
    }
}
