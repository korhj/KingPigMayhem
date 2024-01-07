using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Petrifex : MonoBehaviour, IEnemy, IHasEnemyHealthBar {


    public event EventHandler<IHasEnemyHealthBar.OnHealthUpdateEventArgs> OnHealthUpdate;

    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float petrifexMaxHealth = 10;
    [SerializeField] private float petrifexDamage = 1;

    [SerializeField] private int scoreIncrease = 10;

    private float petrifexHealth;
    private Rigidbody petrifexRigidbody;
    private bool playerIsAlive = true;


    private void Start() {
        petrifexHealth = petrifexMaxHealth;
        petrifexRigidbody = GetComponent<Rigidbody>();
        Player.Instance.OnPlayerDeath += (object sender, EventArgs e) => {playerIsAlive = false;};
    }

    private void Update() {
        
    }

    private void FixedUpdate() {
        if (playerIsAlive) {
            Vector3 moveDir = (Player.Instance.GetPlayerPosition() - transform.position).normalized;
            float moveDistance = moveSpeed * Time.fixedDeltaTime;
            petrifexRigidbody.MovePosition(transform.position + moveDir * moveDistance);
        }
        

    }
    public void IncreasePlayerScoreOnDeath(int scoreIncrease) {
        Player.Instance.IncreaseScore(scoreIncrease);
    }

    public void TakeDamage(Vector3 damageDirection, int knockback, float damage) {
        petrifexRigidbody.AddForce(damageDirection * knockback, ForceMode.Impulse);
        petrifexHealth -= damage;
        OnHealthUpdate?.Invoke(this, new IHasEnemyHealthBar.OnHealthUpdateEventArgs { enemyCurrentHealthNormalized = petrifexHealth / petrifexMaxHealth });
        if (petrifexHealth <= 0) {
            IncreasePlayerScoreOnDeath(scoreIncrease);
            Destroy(gameObject);
        }
    }

    public float GetEnemyDamage() {
        return petrifexDamage;
    }

        
}
