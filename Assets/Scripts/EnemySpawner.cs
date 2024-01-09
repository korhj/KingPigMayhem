using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class EnemySpawner : MonoBehaviour {

    public event EventHandler OnRoomCleared;
    [SerializeField] private Transform petrifex;
    [SerializeField] private Transform frog;
    [SerializeField] private int spawnPointX = 7;
    [SerializeField] private int spawnPointZ = 4;

    private List<UnityEngine.Object> enemies;
    private bool enemiesSpawned = false;
    private void Awake() {

        enemies = new List<UnityEngine.Object>();
        GameManager.Instance.OnStartPlaying += EnemySpawner_SpawnEnemies;
    }

    private void EnemySpawner_SpawnEnemies(object sender, EventArgs e) {
        enemiesSpawned = true;
        enemies.Add(Instantiate(GetRandomEnemy(), new Vector3(-spawnPointX, 0, -spawnPointZ), Quaternion.identity));
        enemies.Add(Instantiate(GetRandomEnemy(), new Vector3(-spawnPointX, 0, spawnPointZ), Quaternion.identity));
        enemies.Add(Instantiate(GetRandomEnemy(), new Vector3(spawnPointX, 0, spawnPointZ), Quaternion.identity));
        enemies.Add(Instantiate(GetRandomEnemy(), new Vector3(spawnPointX, 0, -spawnPointZ), Quaternion.identity));
    }

    private void Update() {
        enemies.RemoveAll(enemy => enemy == null);
        if (enemiesSpawned == true & enemies.Count() == 0) {
            OnRoomCleared?.Invoke(this, EventArgs.Empty);
        }

    }

    private Transform GetRandomEnemy() {
        int randomNumber = UnityEngine.Random.Range(1,3);
        if(randomNumber == 1){
            return petrifex;
        }
        else {
            return frog;
        }
    }

}
