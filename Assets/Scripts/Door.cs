using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Drawing;
using UnityEngine;

public class Door : MonoBehaviour {

    [SerializeField] EnemySpawner enemySpawner;

    private bool isActive;
    private Vector3 playerTeleportPoint;
    private Room connectedRoom;

    void Start() {
        isActive = false;
        enemySpawner.OnEnemiesDead += (object sender, EventArgs e) => {isActive = true;};
    }

    private void OnTriggerEnter(Collider collider) {
        if(isActive & collider.gameObject == Player.Instance.gameObject) {
           Player.Instance.MovePlayerThroughDoor(playerTeleportPoint);
           connectedRoom.playerEntered();
        }
        
    }
    public void setPlayerTeleportPoint(Room room, Vector3 point) {
        playerTeleportPoint = point;
        connectedRoom = room;
        //playerTeleportPoint.x = playerTeleportPoint.x - 14;
    }
}
