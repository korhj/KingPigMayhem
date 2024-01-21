using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BossSpawner : MonoBehaviour, ISpawner
{
    public event EventHandler OnEnemiesDead;

    [SerializeField]
    private Transform kingPig;

    [SerializeField]
    private float roomWidth;

    [SerializeField]
    private float roomHeight;

    private List<UnityEngine.Object> enemies;
    private bool enemiesActive = false;
    private float spawnPointXOfset;
    private float spawnPointZOfset;
    private Vector3 roomPos;

    private void Start()
    {
        enemies = new List<UnityEngine.Object>();
        roomPos = transform.position;
        spawnPointXOfset = roomWidth / 4;
        spawnPointZOfset = roomHeight / 4;
    }

    private void Update()
    {
        enemies.RemoveAll(enemy => enemy == null);
        if (enemiesActive == true & enemies.Count() == 0)
        {
            enemiesActive = false;
            OnEnemiesDead?.Invoke(this, EventArgs.Empty);
            MusicController.Instance.PlayMusic();
            GameManager.Instance.BossKilled();
        }
    }

    public void SpawnEnemies()
    {
        enemiesActive = true;
        enemies.Add(
            Instantiate(
                kingPig,
                roomPos + new Vector3(-spawnPointXOfset, 0, -spawnPointZOfset),
                Quaternion.identity
            )
        );
        MusicController.Instance.PlayBossMusic();
    }
}
