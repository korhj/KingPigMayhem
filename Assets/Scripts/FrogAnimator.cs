using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogAnimator : MonoBehaviour
{
    [SerializeField]
    Frog frog;

    private Animator animator;

    void Awake()
    {
        frog.OnJump += (object sender, EventArgs e) =>
        {
            animator.SetTrigger("FrogJump");
        };
        animator = GetComponent<Animator>();
    }
}
