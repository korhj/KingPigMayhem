using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ChargeAttackProjectile : MonoBehaviour, IAttack {

    [SerializeField] private float chargeAttackSpeed = 2f;
    [SerializeField] private int knockback = 10;
    private Vector3 shootingDir;
    private Rigidbody chargeAttackRigidbody;
    private float charge; 
    private float currentCharge;
    private float movementInTimeStep;

    public void Setup(Vector3 shootingDir, float charge) {
        Debug.Log(GetAngle(shootingDir) + " " + shootingDir);
        chargeAttackRigidbody = GetComponent<Rigidbody>();
        this.shootingDir = shootingDir;
        this.charge = charge;
        currentCharge = 1;
        transform.localScale = new(1, Mathf.Tan(0.5f)*currentCharge, 1);
        transform.eulerAngles = new(0, GetAngle(shootingDir), 90);
        

        Destroy(gameObject, 10f);
    }

    private float GetAngle(Vector3 vector) {
        float n = Mathf.Atan2(vector.x, vector.z) * Mathf.Rad2Deg;
        if (n < 0) n += 360;
        return n;
    }
    private void FixedUpdate() {
        chargeAttackRigidbody.MovePosition(transform.position + shootingDir * chargeAttackSpeed * Time.deltaTime);
        transform.localScale = new(1, Mathf.Tan(0.5f)*currentCharge, 1);
        currentCharge += Time.fixedDeltaTime;
        if (currentCharge >= charge) {
            Destroy(gameObject);
        }

    }

    private void OnTriggerEnter(Collider collider) {
        IEnemy enemy = collider.GetComponent<IEnemy>();
        enemy?.TakeDamage(shootingDir.normalized, knockback, charge);
        //Destroy(gameObject);             
    }
}

