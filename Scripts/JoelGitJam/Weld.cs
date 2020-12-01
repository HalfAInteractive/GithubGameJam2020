using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weld : MonoBehaviour
{
  
    public GameObject follow;
    public Vector3 offset;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.localPosition = follow.transform.position + offset;
    }
}
