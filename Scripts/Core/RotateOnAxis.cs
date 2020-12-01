using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateOnAxis : MonoBehaviour
{
    // degrees spun per frame
    [SerializeField] float speed = 10f;
    [SerializeField] Vector3 axisOfRotation;

    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(transform.position, transform.up, speed);
    }
}
