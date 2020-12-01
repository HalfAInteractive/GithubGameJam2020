using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFocusable
{
    void InLineOfSights();

    void OutOfLineOfSights();
}
