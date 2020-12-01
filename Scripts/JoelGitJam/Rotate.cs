using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    // degrees spun per frame
    [SerializeField] float speed = 10f;

    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(transform.position, transform.forward, speed);
    }
}
