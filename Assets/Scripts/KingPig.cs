using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.Controls;

public class KingPig : MonoBehaviour, IEnemy, IHasEnemyHealthBar
{
    public event EventHandler<IHasEnemyHealthBar.OnHealthUpdateEventArgs> OnHealthUpdate;

    [SerializeField]
    private AudioClip secondPhaseSoundEffect;

    [SerializeField]
    private EnemyHealthBarUI enemyHealthBarUI;

    [SerializeField]
    private GameObject pigGameObject;

    [SerializeField]
    private float baseMoveSpeed = 3f;

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
    private float moveSpeedIncreaseDistance;

    [SerializeField]
    private float speedIncrease;

    [SerializeField]
    private float chargeMaxCooldown;

    [SerializeField]
    private float chargeMaxPreparation;

    private enum KingPigStatus
    {
        charging,
        preparing,
        moving
    }

    private float increasedSpeed;
    private float moveSpeed;
    private float kingPigHealth;
    private Rigidbody kingPigRigidbody;

    private float chargeCooldown;
    private float chargePreparation;
    private bool chargeCollided = false;

    private Vector3 chargeDir;

    private KingPigStatus kingPigStatus;

    private bool pigsSummoned = false;

    private void Start()
    {
        kingPigStatus = KingPigStatus.preparing;
        chargeCooldown = chargeMaxCooldown;
        chargePreparation = idleTime;
        chargeDir = new Vector3(0, 0, 0);
        kingPigHealth = kingPigMaxHealth;
        moveSpeed = baseMoveSpeed;
        increasedSpeed = baseMoveSpeed * speedIncrease;

        kingPigRigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        UpdateSpeed();
        UpdateStatus();
        if (kingPigStatus == KingPigStatus.moving)
            Move();

        if (kingPigStatus == KingPigStatus.preparing)
            Prepare();

        if (kingPigStatus == KingPigStatus.charging)
            Charge();

        //adjust healthBar position and rotation
        enemyHealthBarUI.transform.SetPositionAndRotation(
            kingPigRigidbody.transform.position + new Vector3(0, 2.5f, 1.5f),
            Quaternion.Euler(90, 0, 0)
        );
    }

    private void UpdateSpeed()
    {
        float distance = Vector3.Distance(transform.position, Player.Instance.transform.position);
        if (distance > moveSpeedIncreaseDistance)
        {
            moveSpeed = increasedSpeed;
            return;
        }
        moveSpeed = baseMoveSpeed;
    }

    private void UpdateStatus()
    {
        if (kingPigHealth < kingPigMaxHealth / 2 & !pigsSummoned)
        {
            AudioManager.Instance.PlaySoundEffect(secondPhaseSoundEffect);
            SummonPigs();
        }
        if (chargeCooldown <= 0)
        {
            chargeCooldown = chargeMaxCooldown;
            kingPigStatus = KingPigStatus.preparing;
            return;
        }
        if (chargePreparation <= 0)
        {
            chargePreparation = chargeMaxPreparation;
            kingPigStatus = KingPigStatus.charging;
            return;
        }
        if (chargeCollided)
        {
            chargeCollided = false;
            kingPigStatus = KingPigStatus.moving;
            return;
        }
    }

    private void SummonPigs()
    {
        pigsSummoned = true;
        IRoom currentRoom = GameManager.Instance.GetCurrentRoom();
        Vector3 rightSide =
            transform.position - currentRoom.GetRoomPosition() + transform.right * 2f;
        Vector3 leftSide =
            transform.position - currentRoom.GetRoomPosition() - transform.right * 2f;

        List<(GameObject, Vector3)> summonedPigs = new();
        if (pigGameObject.GetComponent<IEnemy>() == null)
            Debug.LogError("Invalid Pig gameObject");
        summonedPigs.Add((pigGameObject, rightSide));
        summonedPigs.Add((pigGameObject, leftSide));
        currentRoom.SpawnEnemiesToRoom(summonedPigs);
    }

    private void Move()
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

    private void Prepare()
    {
        chargeDir = -(Player.Instance.GetPlayerPosition() - transform.position).normalized;
        chargePreparation -= Time.fixedDeltaTime;
        kingPigRigidbody.transform.rotation = Quaternion.LookRotation(chargeDir, transform.up);
    }

    private void Charge()
    {
        float moveDistance = chargeSpeed * Time.fixedDeltaTime;
        kingPigRigidbody.transform.rotation = Quaternion.LookRotation(chargeDir, transform.up);
        kingPigRigidbody.MovePosition(transform.position - chargeDir * moveDistance);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (
            collision.gameObject != Player.Instance.gameObject
            && collision.gameObject.GetComponent<Pig>() == null
        )
        {
            chargeCollided = true;
        }
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
