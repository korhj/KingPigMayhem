using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour, IAttack {
    [SerializeField] private float fireballSpeed = 10f;
    [SerializeField] private int knockback = 100;
    private Vector3 shootingDir;
    private Rigidbody fireballRigidbody;
    private float damage; 

    public void Setup(Vector3 shootingDir, float damage) {
        fireballRigidbody = GetComponent<Rigidbody>();
        this.shootingDir = shootingDir;
        this.damage = damage;
        Destroy(gameObject, 10f);
    }

    private void FixedUpdate() {
        fireballRigidbody.MovePosition(transform.position + shootingDir * fireballSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider collider) {
        IEnemy enemy = collider.GetComponent<IEnemy>();
        enemy?.TakeDamage(shootingDir.normalized, knockback, damage);
        Destroy(gameObject);             
    }
}
