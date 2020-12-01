using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAmmo
{
    void Fire(Vector3 dir, float speed, float aimDist);
    void Select();
    bool IsFired();
}
