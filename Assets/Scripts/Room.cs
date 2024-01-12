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
    [SerializeField] Light roomLight;

    private bool roomEntered;
    //private List<Door> activeDoors;

    private void Start() {

        roomEntered = false;
        //activeDoors = new List<Door>();

        Room[] adjantedRooms = GameManager.Instance.GetAdjacedRooms(transform.position);

        if(adjantedRooms[0] != null) {
            //activeDoors.Add(doorUp);
            doorUp.setPlayerTeleportPoint(adjantedRooms[0], adjantedRooms[0].transform.position - new Vector3(0,0,5));
        }
        else {
            doorUp.gameObject.SetActive(false);
        }
        if(adjantedRooms[1] != null) {
            //activeDoors.Add(doorDown);
            doorDown.setPlayerTeleportPoint(adjantedRooms[1], adjantedRooms[1].transform.position + new Vector3(0,0,5));
        }
        else {
            doorDown.gameObject.SetActive(false);
        }
        if(adjantedRooms[2] != null) {
            //activeDoors.Add(doorLeft);
            doorLeft.setPlayerTeleportPoint(adjantedRooms[2], adjantedRooms[2].transform.position + new Vector3(14,0,0));
        }
        else {
            doorLeft.gameObject.SetActive(false);
        }
        if(adjantedRooms[3] != null) {
            //activeDoors.Add(doorRight);
            doorRight.setPlayerTeleportPoint(adjantedRooms[3], adjantedRooms[3].transform.position - new Vector3(14,0,0));
        }
        else {
            doorRight.gameObject.SetActive(false);
        }

        roomLight.color = UnityEngine.Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
        

    }

    public void playerEntered() {
        if(!roomEntered) {
            roomEntered = true;
            enemySpawner.SpawnEnemies();
        }
    }

}
