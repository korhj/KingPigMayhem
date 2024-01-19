using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField]
    EnemySpawner enemySpawnerReference;

    [SerializeField]
    Door doorUp;

    [SerializeField]
    Door doorDown;

    [SerializeField]
    Door doorLeft;

    [SerializeField]
    Door doorRight;

    [SerializeField]
    Light roomLight;

    private bool roomEntered;

    private EnemySpawner enemySpawner;

    private void Awake()
    {
        enemySpawner = Instantiate(enemySpawnerReference, transform.position, Quaternion.identity);
        roomEntered = false;
    }

    private void Start()
    {
        Room[] adjantedRooms = GameManager.Instance.GetAdjacedRooms(transform.position);

        if (adjantedRooms[0] != null)
        {
            doorUp.Setup(
                adjantedRooms[0],
                adjantedRooms[0].transform.position - new Vector3(0, 0, 5),
                enemySpawner
            );
        }
        else
        {
            doorUp.gameObject.SetActive(false);
        }
        if (adjantedRooms[1] != null)
        {
            doorDown.Setup(
                adjantedRooms[1],
                adjantedRooms[1].transform.position + new Vector3(0, 0, 5),
                enemySpawner
            );
        }
        else
        {
            doorDown.gameObject.SetActive(false);
        }
        if (adjantedRooms[2] != null)
        {
            doorLeft.Setup(
                adjantedRooms[2],
                adjantedRooms[2].transform.position + new Vector3(14, 0, 0),
                enemySpawner
            );
        }
        else
        {
            doorLeft.gameObject.SetActive(false);
        }
        if (adjantedRooms[3] != null)
        {
            doorRight.Setup(
                adjantedRooms[3],
                adjantedRooms[3].transform.position - new Vector3(14, 0, 0),
                enemySpawner
            );
        }
        else
        {
            doorRight.gameObject.SetActive(false);
        }

        roomLight.color = UnityEngine.Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
    }

    public void SpawnExit(ExitDoor exitDoor)
    {
        Debug.Log(enemySpawner.gameObject.GetInstanceID());
        exitDoor.Setup(enemySpawner);
    }

    public void playerEntered()
    {
        GameManager.Instance.SetCurrentRoomTransform(transform);
        if (roomEntered)
            return;

        roomEntered = true;
        enemySpawner.SpawnEnemies();
    }
}
