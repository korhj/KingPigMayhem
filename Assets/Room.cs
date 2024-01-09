using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;

public class Room : MonoBehaviour {

    [SerializeField] EnemySpawner enemySpawner;
    [SerializeField] Door doorUp;
    [SerializeField] Door doorDown;
    [SerializeField] Door doorLeft;
    [SerializeField] Door doorRight;

    private bool isActive;

    private void Start() {
        enemySpawner.OnRoomCleared += (object sender, EventArgs e) => { Debug.Log("room cleared");};
        //Debug.Log(door);
    }
}
