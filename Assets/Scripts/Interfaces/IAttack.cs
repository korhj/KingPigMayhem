using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttack
{
    public void Setup(Vector3 ShootingDir, float damage);
}
