using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ChargeAttackProjectile : MonoBehaviour, IAttack
{
    [SerializeField]
    private float chargeAttackSpeed = 6f;

    [SerializeField]
    private int knockback = 10;

    [SerializeField]
    private float maxScale = 2.5f;

    [SerializeField]
    private float chargeDamageMultiplier = 2f;

    [SerializeField]
    private float chargeAttackMaxCharge = 4f;

    private Vector3 shootingDir;
    private Rigidbody chargeAttackRigidbody;
    private float charge;
    private float maxLength;
    private float currentScale;
    private float attackMaxScale;
    private Vector3 startingPos;

    public void Setup(Vector3 shootingDir, float charge)
    {
        chargeAttackRigidbody = GetComponent<Rigidbody>();
        this.shootingDir = shootingDir;
        this.charge = charge;
        startingPos = transform.position;
        attackMaxScale = charge / chargeAttackMaxCharge * maxScale;
        maxLength = charge / chargeAttackMaxCharge * (chargeAttackMaxCharge + 1);
        currentScale = Vector3.Distance(transform.position, startingPos) / maxLength * maxScale;
        transform.localScale = new(1, currentScale, 1);
        transform.eulerAngles = new(0, GetAngle(shootingDir), 90);
        //Destroy(gameObject, 10f);
    }

    private float GetAngle(Vector3 vector)
    {
        float n = Mathf.Atan2(vector.x, vector.z) * Mathf.Rad2Deg;
        if (n < 0)
            n += 360;
        return n;
    }

    private void FixedUpdate()
    {
        chargeAttackRigidbody.MovePosition(
            transform.position + shootingDir * chargeAttackSpeed * Time.deltaTime
        );
        currentScale = Vector3.Distance(transform.position, startingPos) / maxLength * maxScale;
        transform.localScale = new(1, currentScale, 1);
        if (currentScale >= attackMaxScale)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        IEnemy enemy = collider.GetComponent<IEnemy>();
        enemy?.TakeDamage(shootingDir.normalized, knockback, chargeDamageMultiplier * charge);
        //Destroy(gameObject);
    }
}
