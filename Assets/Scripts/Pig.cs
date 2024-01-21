using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pig : MonoBehaviour, IEnemy, IHasEnemyHealthBar
{
    public event EventHandler<IHasEnemyHealthBar.OnHealthUpdateEventArgs> OnHealthUpdate;

    [SerializeField]
    EnemyHealthBarUI enemyHealthBarUI;

    [SerializeField]
    private float moveSpeed = 3f;

    [SerializeField]
    private float pigMaxHealth = 10;

    [SerializeField]
    private float pigDamage = 1;

    [SerializeField]
    private int scoreIncrease = 10;

    private float pigHealth;
    private Rigidbody pigRigidbody;

    //private Transform healthbarTransform;

    private void Start()
    {
        pigHealth = pigMaxHealth;
        pigRigidbody = GetComponent<Rigidbody>();
        /*
        Player.Instance.OnPlayerDeath += (object sender, EventArgs e) =>
        {
            playerIsAlive = false;
        };
        */
    }

    private void Update() { }

    private void FixedUpdate()
    {
        //if (playerIsAlive)
        Vector3 moveDir = (Player.Instance.GetPlayerPosition() - transform.position).normalized;
        float moveDistance = moveSpeed * Time.fixedDeltaTime;
        pigRigidbody.MovePosition(transform.position + moveDir * moveDistance);
        pigRigidbody.transform.rotation = Quaternion.LookRotation(-moveDir, transform.up);
        enemyHealthBarUI.transform.SetPositionAndRotation(
            pigRigidbody.transform.position + new Vector3(0, 2.5f, 1),
            Quaternion.Euler(90, 0, 0)
        );
    }

    public void IncreasePlayerScoreOnDeath(int scoreIncrease)
    {
        Player.Instance.IncreaseScore(scoreIncrease);
    }

    public void TakeDamage(Vector3 damageDirection, int knockback, float damage)
    {
        pigRigidbody.AddForce(damageDirection * knockback, ForceMode.Impulse);
        pigHealth -= damage;
        OnHealthUpdate?.Invoke(
            this,
            new IHasEnemyHealthBar.OnHealthUpdateEventArgs
            {
                enemyCurrentHealthNormalized = pigHealth / pigMaxHealth
            }
        );
        if (pigHealth <= 0)
        {
            IncreasePlayerScoreOnDeath(scoreIncrease);
            Destroy(gameObject);
        }
    }

    public float GetEnemyDamage()
    {
        return pigDamage;
    }
}
