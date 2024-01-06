using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemy {

    public event EventHandler<OnHealthUpdateEventArgs> OnHealthUpdate;
    public class OnHealthUpdateEventArgs : EventArgs {
        public float enemyCurrentHealthNormalized;
    }
    void TakeDamage(Vector3 damageDirection, int knockback);
}
