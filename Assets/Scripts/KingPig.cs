using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;
using UnityEngine.InputSystem.Controls;

public class KingPig : MonoBehaviour, IEnemy, IHasEnemyHealthBar
{
    public event EventHandler<IHasEnemyHealthBar.OnHealthUpdateEventArgs> OnHealthUpdate;

    [SerializeField]
    EnemyHealthBarUI enemyHealthBarUI;

    [SerializeField]
    private float moveSpeed = 3f;

    [SerializeField]
    private float chargeSpeed = 3f;

    [SerializeField]
    private float kingPigMaxHealth = 10;

    [SerializeField]
    private float kingPigDamage = 1;

    [SerializeField]
    private int scoreIncrease = 50;

    [SerializeField]
    private float idleTime;

    [SerializeField]
    private float chargeMaxCooldown;

    [SerializeField]
    private float chargeMaxPreparation;

    [SerializeField]
    private float chargeMaxDuration;

    private float kingPigHealth;
    private Rigidbody kingPigRigidbody;

    private float chargeCooldown;
    private float chargePreparation;
    private float chargeDuration;

    private Vector3 chargeDir;

    private enum KingPigStatus
    {
        charging,
        preparing,
        moving
    }

    private KingPigStatus kingPigStatus;

    //private Transform healthbarTransform;

    private void Start()
    {
        kingPigStatus = KingPigStatus.preparing;
        chargeCooldown = chargeMaxCooldown;
        chargePreparation = idleTime;
        chargeDuration = chargeMaxDuration;
        chargeDir = new Vector3(0, 0, 0);
        kingPigHealth = kingPigMaxHealth;
        kingPigRigidbody = GetComponent<Rigidbody>();
        /*
        Player.Instance.OnPlayerDeath += (object sender, EventArgs e) =>
        {
            playerIsAlive = false;
        };
        */
    }

    private void FixedUpdate()
    {
        if (chargeCooldown <= 0)
        {
            chargeCooldown = chargeMaxCooldown;
            kingPigStatus = KingPigStatus.preparing;
        }
        if (chargePreparation <= 0)
        {
            chargePreparation = chargeMaxPreparation;
            kingPigStatus = KingPigStatus.charging;
        }
        if (chargeDuration <= 0)
        {
            chargeDuration = chargeMaxDuration;
            kingPigStatus = KingPigStatus.moving;
        }

        if (kingPigStatus == KingPigStatus.moving)
        {
            Vector3 moveDir = (Player.Instance.GetPlayerPosition() - transform.position).normalized;
            float moveDistance = moveSpeed * Time.fixedDeltaTime;
            kingPigRigidbody.MovePosition(transform.position + moveDir * moveDistance);
            kingPigRigidbody.transform.rotation = Quaternion.LookRotation(-moveDir, transform.up);
            enemyHealthBarUI.transform.SetPositionAndRotation(
                kingPigRigidbody.transform.position + new Vector3(0, 2.5f, 1),
                Quaternion.Euler(90, 0, 0)
            );
            chargeCooldown -= Time.fixedDeltaTime;
        }

        if (kingPigStatus == KingPigStatus.preparing)
        {
            chargeDir = -(Player.Instance.GetPlayerPosition() - transform.position).normalized;
            chargePreparation -= Time.fixedDeltaTime;
            kingPigRigidbody.transform.rotation = Quaternion.LookRotation(chargeDir, transform.up);
        }
        if (kingPigStatus == KingPigStatus.charging)
        {
            chargeDuration -= Time.fixedDeltaTime;
            float moveDistance = chargeSpeed * Time.fixedDeltaTime;
            kingPigRigidbody.transform.rotation = Quaternion.LookRotation(chargeDir, transform.up);
            kingPigRigidbody.MovePosition(transform.position - chargeDir * moveDistance);
        }

        enemyHealthBarUI.transform.SetPositionAndRotation(
            kingPigRigidbody.transform.position + new Vector3(0, 2.5f, 1.5f),
            Quaternion.Euler(90, 0, 0)
        );
    }

    public void IncreasePlayerScoreOnDeath(int scoreIncrease)
    {
        Player.Instance.IncreaseScore(scoreIncrease);
    }

    public void TakeDamage(Vector3 damageDirection, int knockback, float damage)
    {
        kingPigRigidbody.AddForce(damageDirection * knockback, ForceMode.Impulse);
        kingPigHealth -= damage;
        OnHealthUpdate?.Invoke(
            this,
            new IHasEnemyHealthBar.OnHealthUpdateEventArgs
            {
                enemyCurrentHealthNormalized = kingPigHealth / kingPigMaxHealth
            }
        );
        if (kingPigHealth <= 0)
        {
            IncreasePlayerScoreOnDeath(scoreIncrease);
            Destroy(gameObject);
        }
    }

    public float GetEnemyDamage()
    {
        return kingPigDamage;
    }
}
