using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobeRing : MonoBehaviour
{
    
    [SerializeField]
    float numGlobes = 0;    
    [SerializeField]
    float radius = 1;
    
    [SerializeField]
    GameObject globe;
    
    // Start is called before the first frame update
    void Start()
    {
        float angle = 2 * Mathf.PI / numGlobes;
        for(int i = 0; i < numGlobes; i++)
        {
            GameObject g = Instantiate(globe, transform.position + new Vector3(Mathf.Cos(i * angle + Mathf.PI/2) * radius, Mathf.Sin(i * angle + Mathf.PI/2) * radius, 0), Quaternion.identity);
            g.transform.parent = transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
