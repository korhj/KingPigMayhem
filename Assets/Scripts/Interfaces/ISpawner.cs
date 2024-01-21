using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISpawner
{
    public event EventHandler OnEnemiesDead;

    public void SpawnEnemies();
}
