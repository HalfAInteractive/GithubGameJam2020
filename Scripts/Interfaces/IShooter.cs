using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IShooter
{
    void Fire();
    
    void Reload(IAmmo ammo);
}
