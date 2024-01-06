using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{

    public static Player Instance { get; private set; }

    public event EventHandler<OnHealthUpdateEventArgs> OnHealthUpdate;
    public event EventHandler<OnShootEventArgs> OnShoot; 
    public event EventHandler OnPlayerDeath;

    public class OnHealthUpdateEventArgs : EventArgs {
        public float playerCurrentHealthNormalized;
    }

    public class OnShootEventArgs : EventArgs {
        public Vector3 shooterPos;
        public Vector3 shootingDir;
    }

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private float playerMaxHealth = 20f;
    [SerializeField] private float invulnerabilityTime = 1f;
    [SerializeField] private float attackSpeed = 1f;

    private bool playerIsAlive = true;
    private Rigidbody playerRigidbody;
    private float playerHealth = 0f;
    private float timeSinceDamage = 0f;
    private float timeSinceAttack = 0f;

    private void Awake() {
        if (Instance != null){
            Debug.Log("Player instance already exists");
        }
        Instance = this;
    }

    private void Start() {
        playerHealth = playerMaxHealth;
        playerRigidbody = GetComponent<Rigidbody>();
    }

    private void Update() {
    }

    private void FixedUpdate() {
        if(playerIsAlive) {
            Vector2 movementInputVector = gameInput.GetMovementVectorNormalized();
            Vector3 moveDir = new Vector3(movementInputVector.x, 0f, movementInputVector.y).normalized;
            float moveDistance = moveSpeed * Time.fixedDeltaTime;
            playerRigidbody.MovePosition(transform.position + moveDir * moveDistance);

            Vector2 aimInputVector = gameInput.GetAimVectorNormalized();
            Vector3 aimDir = new Vector3(aimInputVector.x, 0f, aimInputVector.y).normalized;
            if(aimDir.magnitude > 0.9) {
                Attack(aimDir);
            }

            timeSinceDamage += Time.fixedDeltaTime;
            timeSinceAttack += Time.fixedDeltaTime;
        }
    }


    private void OnCollisionEnter(Collision collision) {
        if(collision.collider.GetComponent<IEnemy>() != null)TakeDamage(collision.collider.GetComponent<IEnemy>());
    }

    private void OnCollisionStay(Collision collision) {
        if(collision.collider.GetComponent<IEnemy>() != null)TakeDamage(collision.collider.GetComponent<IEnemy>());
    }

    private void TakeDamage(IEnemy enemy) {
        if(timeSinceDamage > invulnerabilityTime) {
                playerHealth -= enemy.GetEnemyDamage();
                OnHealthUpdate?.Invoke(this, new OnHealthUpdateEventArgs { playerCurrentHealthNormalized = playerHealth / playerMaxHealth });
                timeSinceDamage = 0;
                if (playerHealth <= 0) {
                    playerIsAlive = false;
                    OnPlayerDeath?.Invoke(this, EventArgs.Empty);
                }
        }
        
    }

    private void Attack(Vector3 aimDir)
    {
        if(timeSinceAttack > attackSpeed) {
            timeSinceAttack = 0;
            OnShoot?.Invoke(this, new OnShootEventArgs { shooterPos = transform.position + aimDir, shootingDir = aimDir });
        }
    }

    public Vector3 GetPlayerPosition() {
        return transform.position;
    }
    
    public float GetPlayerHealth() {
        return playerHealth;
    }

}
