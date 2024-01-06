using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    [SerializeField] private float fireballSpeed = 10f;
    [SerializeField] private readonly int knockback = 100;
    private Vector3 shootingDir;
    private Rigidbody fireballRigidbody;
    private bool hasCollided = false;

    public void Setup(Vector3 shootingDir) {
        fireballRigidbody = GetComponent<Rigidbody>();
        this.shootingDir = shootingDir;
        Destroy(gameObject, 10f);
    }

    private void FixedUpdate() {
        fireballRigidbody.MovePosition(transform.position + shootingDir * fireballSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider collider) {
        if(!hasCollided){
            IEnemy enemy = collider.GetComponent<IEnemy>();
            enemy?.TakeDamage(shootingDir.normalized, knockback);
            Destroy(gameObject);    
            hasCollided = true;
        }
         
    }
}
