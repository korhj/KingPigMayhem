using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        Player.Instance.OnShoot += (object sender, Player.OnShootEventArgs e) =>
        {
            animator.SetTrigger("PlayerShoot");
        };
        Player.Instance.OnShootChargeAttack += (object sender, Player.OnShootEventArgs e) =>
        {
            animator.SetTrigger("PlayerShoot");
        };
    }
}
