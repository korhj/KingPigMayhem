using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Petrifex : MonoBehaviour, IEnemy {


    public event EventHandler<IEnemy.OnHealthUpdateEventArgs> OnHealthUpdate;

    public class OnHealthUpdateEventArgs : EventArgs {
        public float enemyCurrentHealthNormalized;
    }

    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float petrifexMaxHealth = 10;

    private float petrifexHealth;
    private Rigidbody petrifexRigidbody;


    private void Start() {
        petrifexHealth = petrifexMaxHealth;
        petrifexRigidbody = GetComponent<Rigidbody>();
    }

    private void Update() {
        
    }

    private void FixedUpdate() {
        
        Vector3 moveDir = Player.Instance.GetPlayerPosition() - transform.position;
        float moveDistance = moveSpeed * Time.fixedDeltaTime;
        petrifexRigidbody.MovePosition(transform.position + moveDir * moveDistance);

    }

    public void TakeDamage(Vector3 damageDirection, int knockback) {
        petrifexRigidbody.AddForce(damageDirection * knockback, ForceMode.Impulse);
        petrifexHealth -= 5;
        OnHealthUpdate?.Invoke(this, new IEnemy.OnHealthUpdateEventArgs { enemyCurrentHealthNormalized = petrifexHealth / petrifexMaxHealth });
        if (petrifexHealth <= 0) {
            Destroy(gameObject);
        }
    }

        
}
