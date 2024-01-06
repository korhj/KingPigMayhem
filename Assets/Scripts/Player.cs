using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public static Player Instance { get; private set; }

    public event EventHandler<OnHealthUpdateEventArgs> OnHealthUpdate;
    public event EventHandler<OnShootEventArgs> OnShoot; 

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


    private void OnCollisionEnter(Collision collision) {
        if(collision.gameObject.name != null)TakeDamage(collision.gameObject.name);
    }

    private void OnCollisionStay(Collision collision) {
        if(collision.gameObject.name != null)TakeDamage(collision.gameObject.name);
    }

    private void TakeDamage(string name) {
        if(timeSinceDamage > invulnerabilityTime) {
            if (name == "Petrifex(Clone)"){
                playerHealth -= 1;
                OnHealthUpdate?.Invoke(this, new OnHealthUpdateEventArgs { playerCurrentHealthNormalized = playerHealth / playerMaxHealth });
                timeSinceDamage = 0;
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
