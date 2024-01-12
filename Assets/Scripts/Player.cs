using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour {

    public static Player Instance { get; private set; }

    public event EventHandler<OnHealthUpdateEventArgs> OnHealthUpdate;
    public event EventHandler<OnShootEventArgs> OnShoot; 
    public event EventHandler OnPlayerDeath;
    public event EventHandler<OnChargeUpdateEventArgs> OnChargeUpdate;

    public event EventHandler<OnShootEventArgs> OnShootChargeAttack;
    public event EventHandler<OnScoreIncreaseEventArgs> OnScoreIncrease;

    public class OnHealthUpdateEventArgs : EventArgs {
        public float playerCurrentHealthNormalized;
    }

    public class OnShootEventArgs : EventArgs {
        public Vector3 shooterPos;
        public Vector3 shootingDir;
        public float damage;
    }

    public class OnChargeUpdateEventArgs : EventArgs {
        public float charge;
    }
    public class OnScoreIncreaseEventArgs : EventArgs{
        public int score;
    }

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private float playerMaxHealth = 20f;
    [SerializeField] private float invulnerabilityTime = 1f;
    [SerializeField] private float attackSpeed = 1f;
    [SerializeField] private GameObject chargeAttackVisual;
    [SerializeField] private float chargeAttackMaxCharge = 4f;
    [SerializeField] private float shootDamage = 5f;

    private bool playerIsAlive = true;
    private Rigidbody playerRigidbody;
    private float playerHealth = 0f;
    private float timeSinceDamage = 0f;
    private float timeSinceAttack = 0f;
    private float chargeAttackCharge = 0f;
    private Vector3 chargeDir;
    private float score;

    private void Awake() {
        if (Instance != null){
            Debug.Log("Player instance already exists");
        }
        Instance = this;
        score = 0;
    }

    private void Start() {
        playerHealth = playerMaxHealth;
        playerRigidbody = GetComponent<Rigidbody>();
        chargeAttackVisual.SetActive(false);
    }

    private void FixedUpdate() {
        if(playerIsAlive) {
            Vector2 movementInputVector = gameInput.GetMovementVectorNormalized();
            Vector3 moveDir = new Vector3(movementInputVector.x, 0f, movementInputVector.y).normalized;
            float moveDistance = moveSpeed * Time.fixedDeltaTime;
            playerRigidbody.MovePosition(transform.position + moveDir * moveDistance);

            Vector2 chargeAttackInputVector = gameInput.GetChargeAttackVectorNormalized();
            Vector3 chargeAttackDir = new Vector3(chargeAttackInputVector.x, 0f, chargeAttackInputVector.y).normalized;
            if(chargeAttackDir.magnitude > 0.9) {
                chargeAttackVisual.SetActive(true);
                playerRigidbody.rotation = Quaternion.LookRotation(chargeAttackDir, transform.up);
                if( chargeAttackCharge < chargeAttackMaxCharge) {
                    chargeAttackCharge += Time.fixedDeltaTime;
                    chargeDir = chargeAttackDir;
                    OnChargeUpdate?.Invoke(this, new OnChargeUpdateEventArgs {charge = chargeAttackCharge});
                }
            }
            else {
                if(chargeAttackCharge != 0) {
                    ChargeAttack(chargeDir);
                    chargeAttackCharge = 0f;
                    OnChargeUpdate?.Invoke(this, new OnChargeUpdateEventArgs {charge = chargeAttackCharge});
                }
                chargeAttackVisual.SetActive(false);
            }

            Vector2 aimInputVector = gameInput.GetAimVectorNormalized();
            Vector3 aimDir = new Vector3(aimInputVector.x, 0f, aimInputVector.y).normalized;
            if(aimDir.magnitude > 0.9 & chargeAttackDir.magnitude < 0.9) {
                playerRigidbody.rotation = Quaternion.LookRotation(aimDir, transform.up);
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
            OnShoot?.Invoke(this, new OnShootEventArgs { shooterPos = transform.position + aimDir, shootingDir = aimDir, damage = shootDamage});
        }
    }
    private void ChargeAttack(Vector3 chargeDir) {
        OnShootChargeAttack?.Invoke(this, new OnShootEventArgs { shooterPos = transform.position + chargeDir, shootingDir = chargeDir, damage = chargeAttackCharge});
    }

    public Vector3 GetPlayerPosition() {
        return transform.position;
    }
    
    public float GetPlayerHealth() {
        return playerHealth;
    }

    public void IncreaseScore(float scoreIncrese) {
        this.score += scoreIncrese;
        OnScoreIncrease?.Invoke(this, new OnScoreIncreaseEventArgs { score = (int)this.score});
    }

    public int GetScore() {
        return (int)score;
    }

    public void MovePlayerThroughDoor(Vector3 point) {
        transform.position = point;
    }
}
