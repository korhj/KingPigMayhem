using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public event EventHandler<OnEnemiesActiveEventArgs> OnEnemiesActive;

    public class OnEnemiesActiveEventArgs : EventArgs
    {
        public bool active;
    }

    /*
    [SerializeField]
    private Transform pig;

    [SerializeField]
    private Transform frog;

    [SerializeField]
    private float roomWidth;

    [SerializeField]
    private float roomHeight;
    */
    private List<GameObject> enemies;
    private bool enemiesActive = false;

    //private float spawnPointXOfset;
    //private float spawnPointZOfset;
    //private Vector3 roomPos;

    private void Start()
    {
        enemies = new List<GameObject>();
        //roomPos = transform.position;
        //spawnPointXOfset = roomWidth / 4;
        //spawnPointZOfset = roomHeight / 4;
    }

    private void Update()
    {
        enemies.RemoveAll(enemy => enemy == null);
        if (enemiesActive == true & enemies.Count() == 0)
        {
            enemiesActive = false;
            OnEnemiesActive?.Invoke(this, new OnEnemiesActiveEventArgs { active = enemiesActive });
        }
    }

    /*
    private Transform GetRandomEnemy()
    {
        int randomNumber = UnityEngine.Random.Range(1, 3);
        if (randomNumber == 1)
        {
            return pig;
        }
        else
        {
            return frog;
        }
    }
    */
    public void SpawnEnemies(List<(GameObject, Vector3)> spawnableEnemies)
    {
        enemiesActive = true;
        OnEnemiesActive?.Invoke(this, new OnEnemiesActiveEventArgs { active = enemiesActive });

        foreach ((GameObject, Vector3) enemy in spawnableEnemies)
        {
            enemies.Add(
                Instantiate(enemy.Item1, transform.position + enemy.Item2, Quaternion.identity)
            );
        }
        /*
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
        */
    }
}
