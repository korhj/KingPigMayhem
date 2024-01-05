using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    [SerializeField] private float fireballSpeed = 10f;
    private Vector3 shootingDir;
    public void Setup(Vector3 shootingDir) {
        this.shootingDir = shootingDir;
        Destroy(gameObject, 10f);
    }

    private void Update() {
        transform.position += shootingDir * fireballSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider collider) {
        Debug.Log("OnTrigger");
        IEnemy enemy = collider.GetComponent<IEnemy>();
        if ( enemy != null) {
            enemy.TakeDamage();
        }
        
        Destroy(gameObject);
        
    }
}
