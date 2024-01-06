using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Petrifex : MonoBehaviour, IEnemy
{
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

    public void TakeDamage() {
        Debug.Log("TakeDamage");
        petrifexHealth -= 5;
        if (petrifexHealth <= 0) {
            Destroy(gameObject);
        }
    }

        
}
