using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frog : MonoBehaviour, IEnemy, IHasEnemyHealthBar {
    public event EventHandler<IHasEnemyHealthBar.OnHealthUpdateEventArgs> OnHealthUpdate;

    [SerializeField] private float jumpSpeed = 10f;
    [SerializeField] private float jumpDuration = 0.3f;
    [SerializeField] private float timeBetweenJumps = 0.5f;
    [SerializeField] private float frogMaxHealth = 10f;
    [SerializeField] private float directionNoiceCutoffDistance = 5f;
    [SerializeField] private float directionNoiceLevel = 1.5f;
    [SerializeField] private float frogDamage = 2;
    [SerializeField] private int scoreIncrease = 10;

    private float frogHealth;
    private Rigidbody frogRigidbody;
    private float jumpStarted = 0f;
    private float jumpEnded = 0f;
    private bool currentlyJumping = true;
    private Vector3 moveDir;
    private bool playerIsAlive = true;


    private void Start() {
        frogHealth = frogMaxHealth;
        frogRigidbody = GetComponent<Rigidbody>();
        moveDir = GetMovementDir();
        Player.Instance.OnPlayerDeath += (object sender, EventArgs e) => {playerIsAlive = false;};
    }

    private void Update() {
        
    }

    private void FixedUpdate() {
        if (playerIsAlive) {
            if(currentlyJumping & jumpStarted < jumpDuration) {
                jumpStarted += Time.fixedDeltaTime;
                float moveDistance = jumpSpeed * Time.fixedDeltaTime;
                frogRigidbody.MovePosition(transform.position + moveDir * moveDistance);
            } 
            else if (currentlyJumping & jumpStarted > jumpDuration) {
                frogRigidbody.velocity = Vector3.zero;
                currentlyJumping = false;
                jumpEnded = 0f;
            }
            else if (!currentlyJumping & jumpEnded < timeBetweenJumps) {
                jumpEnded += Time.fixedDeltaTime;
            }
            else if (!currentlyJumping & jumpEnded > timeBetweenJumps) {
                currentlyJumping = true;
                jumpStarted = 0f;
                moveDir = GetMovementDir();
            }
            else {
                Debug.LogError("Error in frog jumping logic");
            } 
        }
        

    }

    private Vector3 GetMovementDir() {
        Vector3 playerPos = Player.Instance.GetPlayerPosition();
        Vector3 moveDir = (playerPos - transform.position).normalized;
        if (Vector3.Distance(playerPos, transform.position) > directionNoiceCutoffDistance) {
            moveDir = AddNoice(moveDir);
         }
        return moveDir;
    }

    private Vector3 AddNoice(Vector3 dir) {
        dir.x += UnityEngine.Random.Range(-directionNoiceLevel, directionNoiceLevel);
        dir.z += UnityEngine.Random.Range(-directionNoiceLevel, directionNoiceLevel);
        return dir.normalized;
    }
    public void IncreasePlayerScoreOnDeath(int scoreIncrease) {
        Player.Instance.IncreaseScore(scoreIncrease);
    }

    public void TakeDamage(Vector3 damageDirection, int knockback, float damage) {
        frogRigidbody.AddForce(damageDirection * knockback, ForceMode.Impulse);
        frogHealth -= damage;
        OnHealthUpdate?.Invoke(this, new IHasEnemyHealthBar.OnHealthUpdateEventArgs { enemyCurrentHealthNormalized = frogHealth / frogMaxHealth });
        if (frogHealth <= 0) {
            IncreasePlayerScoreOnDeath(scoreIncrease);
            Destroy(gameObject);
        }
    }

    public float GetEnemyDamage() {
        return frogDamage;
    }
}
