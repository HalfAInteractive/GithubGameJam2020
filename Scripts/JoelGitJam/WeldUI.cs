using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeldUI : MonoBehaviour
{
    public Camera cam = null;
    public GameObject follow;
    public Vector3 offset;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
      transform.position = cam.WorldToScreenPoint(follow.transform.position + offset);
    }
}
