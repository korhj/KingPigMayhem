using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public event EventHandler OnEnemiesDead;

    [SerializeField]
    private Transform petrifex;

    [SerializeField]
    private Transform frog;

    [SerializeField]
    private float roomWidth;

    [SerializeField]
    private float roomHeight;

    [SerializeField]
    private Room parentRoom;

    private List<UnityEngine.Object> enemies;
    private bool enemiesActive = false;
    private float spawnPointXOfset;
    private float spawnPointZOfset;
    private Vector3 roomPos;

    private void Start()
    {
        enemies = new List<UnityEngine.Object>();
        roomPos = parentRoom.transform.position;
        spawnPointXOfset = roomWidth / 4;
        spawnPointZOfset = roomHeight / 4;
        //Debug.Log(roomPos + " " + spawnPointXOfset + " " + spawnPointZOfset);
        //SpawnEnemies();
    }

    private void Update()
    {
        enemies.RemoveAll(enemy => enemy == null);
        if (enemiesActive == true & enemies.Count() == 0)
        {
            enemiesActive = false;
            OnEnemiesDead?.Invoke(this, EventArgs.Empty);
        }
    }

    private Transform GetRandomEnemy()
    {
        int randomNumber = UnityEngine.Random.Range(1, 3);
        if (randomNumber == 1)
        {
            return petrifex;
        }
        else
        {
            return frog;
        }
    }

    public void SpawnEnemies()
    {
        enemiesActive = true;
        enemies.Add(
            Instantiate(
                GetRandomEnemy(),
                roomPos + new Vector3(-spawnPointXOfset, 0, -spawnPointZOfset),
                Quaternion.identity
            )
        );
        enemies.Add(
            Instantiate(
                GetRandomEnemy(),
                roomPos + new Vector3(-spawnPointXOfset, 0, spawnPointZOfset),
                Quaternion.identity
            )
        );
        enemies.Add(
            Instantiate(
                GetRandomEnemy(),
                roomPos + new Vector3(spawnPointXOfset, 0, spawnPointZOfset),
                Quaternion.identity
            )
        );
        enemies.Add(
            Instantiate(
                GetRandomEnemy(),
                roomPos + new Vector3(spawnPointXOfset, 0, -spawnPointZOfset),
                Quaternion.identity
            )
        );
    }
}
