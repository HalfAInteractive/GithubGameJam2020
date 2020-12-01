using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayer : IMoveable, IDamageable, IHealable, IKillable, IShooter, ICollector
{
    Vector3 GetPosition();
}