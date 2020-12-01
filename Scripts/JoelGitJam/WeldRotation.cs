using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeldRotation : MonoBehaviour
{
    public GameObject follow;
    public Vector3 rotationOffset;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
      transform.rotation = follow.transform.rotation * Quaternion.Euler(rotationOffset);
    }
}
