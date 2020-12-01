using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// IAttractor sounds like a more fitting name. IPushable sounds like IMoveable.
public interface IPushable
{
    void Push(Vector3 dir);
}
