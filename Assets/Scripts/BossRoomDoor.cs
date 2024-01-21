using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Drawing;
using UnityEngine;

public class BossRoomDoor : MonoBehaviour
{
    private bool isActive;
    private Vector3 playerTeleportPoint;
    private IRoom connectedRoom;

    public void Setup(IRoom room, Vector3 point, ISpawner enemySpawner)
    {
        isActive = false;
        enemySpawner.OnEnemiesDead += (object sender, EventArgs e) =>
        {
            isActive = true;
        };
        playerTeleportPoint = point;
        connectedRoom = room;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (
            isActive
            & collider.gameObject == Player.Instance.gameObject
            & Player.Instance.KeyCollected()
        )
        {
            Player.Instance.MovePlayerThroughDoor(playerTeleportPoint);
            connectedRoom.PlayerEntered(transform.position);
        }
    }
}