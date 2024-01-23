using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHasEnemyHealthBar
{
    public event EventHandler<OnHealthUpdateEventArgs> OnHealthUpdate;

    public class OnHealthUpdateEventArgs : EventArgs
    {
        public float enemyCurrentHealthNormalized;
    }
}
