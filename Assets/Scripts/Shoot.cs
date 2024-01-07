using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{

    [SerializeField] private Transform fireball;
    [SerializeField] private Transform chargeAttack;
    [SerializeField] private float x;
    [SerializeField] private float y;
    [SerializeField] private float z;

    void Awake() {
        Player.Instance.OnShoot += Shoot_OnShoot;
        Player.Instance.OnShootChargeAttack += Shoot_OnShootChargeAttack;
    }
    private void Shoot_OnShootChargeAttack(object sender, Player.OnShootEventArgs e) {
        Transform chargeAttackTransform = Instantiate(chargeAttack, e.shooterPos, Quaternion.identity);
        chargeAttackTransform.GetComponent<ChargeAttackProjectile>().Setup(e.shootingDir, e.damage);
    }

    private void Shoot_OnShoot(object sender, Player.OnShootEventArgs e) {
        Transform fireballTransform = Instantiate(fireball, e.shooterPos, Quaternion.identity);
        fireballTransform.GetComponent<Fireball>().Setup(e.shootingDir, e.damage);
    }

}