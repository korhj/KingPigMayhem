using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class Shoot : MonoBehaviour
{

    [SerializeField] private Transform fireball;

    void Awake() {
        GetComponent<Player>().OnShoot += Shoot_OnShoot;
    }

    private void Shoot_OnShoot(object sender, Player.OnShootEventArgs e) {
        Transform fireballTransform = Instantiate(fireball, e.shooterPos, UnityEngine.Quaternion.identity);
        fireballTransform.GetComponent<Fireball>().Setup(e.shootingDir);
    }

}
