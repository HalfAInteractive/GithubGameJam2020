using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMoveable
{
    void Boost(float amount, ForceMode forceMode);

    void Bounce(float amount, Vector3 targetPos, ForceMode forceMode);
}
