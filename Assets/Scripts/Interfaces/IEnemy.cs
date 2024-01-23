using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemy
{
    float GetEnemyDamage();
    void TakeDamage(Vector3 damageDirection, int knockback, float damage);
    void IncreasePlayerScoreOnDeath(int scoreIncrease);
}
