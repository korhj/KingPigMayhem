using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemy {
    void TakeDamage(Vector3 damageDirection, int knockback);
}
