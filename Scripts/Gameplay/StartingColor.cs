using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartingColor : MonoBehaviour
{
    [SerializeField]  Color startingColor = Color.white;

    // Start is called before the first frame update
    void Start()
    {
        Material m = GetComponent<Renderer>().material;
        m.color = startingColor;
    }
}
