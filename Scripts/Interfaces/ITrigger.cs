using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITrigger
{
    void Trigger();
    void SetId(int i, ITriggerManager m);
    void Broadcast(int data);
    void SetLock(bool l);
}
