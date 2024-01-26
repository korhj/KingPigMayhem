using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Room : MonoBehaviour, IRoom
{
    [SerializeField]
    EnemySpawner enemySpawnerReference;

    [SerializeField]
    private List<GameObject> enemyTypes;

    [SerializeField]
    private List<Vector3> enemyPositions;

    [SerializeField]
    private float doorXoffset;

    [SerializeField]
    private float doorYffset;

    [SerializeField]
    private float doorZffset;

    [SerializeField]
    Light roomLight;

    [SerializeField]
    Door doorReference;

    [SerializeField]
    BossRoomDoor bossRoomDoorReference;

    private bool roomEntered;

    private EnemySpawner enemySpawner;
    private (Vector3, Quaternion)[] doorOffsets;

    private List<(GameObject, Vector3)> enemiesToSpawn;

    private void Awake()
    {
        enemySpawner = Instantiate(enemySpawnerReference, transform.position, Quaternion.identity);
        roomEntered = false;

        doorOffsets = new (Vector3 Position, Quaternion Rotation)[]
        {
            (new Vector3(0, doorYffset, doorZffset), Quaternion.Euler(0, 0, 0)), // Up: Position and Rotation
            (new Vector3(0, doorYffset, -doorZffset), Quaternion.Euler(0, 0, 0)), // Down: Position and Rotation
            (new Vector3(-doorXoffset, doorYffset, 0), Quaternion.Euler(0, 90, 0)), // Left: Position and Rotation
            (new Vector3(doorXoffset, doorYffset, 0), Quaternion.Euler(0, -90, 0)) // Right: Position and Rotation
        };

        InitializeEnemiesToSpawn();
    }

    private void Start()
    {
        IRoom[] adjantedRooms = GameManager.Instance.GetAdjacedRooms(transform.position);

        for (int i = 0; i < adjantedRooms.Length; i++)
        {
            if (adjantedRooms[i] != null)
            {
                if (adjantedRooms[i] is BossRoom)
                {
                    BossRoomDoor door = Instantiate(
                        bossRoomDoorReference,
                        transform.position + doorOffsets[i].Item1,
                        doorOffsets[i].Item2
                    );
                    door.Setup(
                        adjantedRooms[i],
                        GetPlayerSpawnPoint(
                            adjantedRooms[i].GetRoomPosition(),
                            doorOffsets[i].Item1
                        ),
                        enemySpawner
                    );
                }
                else
                {
                    Door door = Instantiate(
                        doorReference,
                        transform.position + doorOffsets[i].Item1,
                        doorOffsets[i].Item2
                    );
                    door.Setup(
                        adjantedRooms[i],
                        GetPlayerSpawnPoint(
                            adjantedRooms[i].GetRoomPosition(),
                            doorOffsets[i].Item1
                        ),
                        enemySpawner
                    );
                }
            }

            roomLight.color = UnityEngine.Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
        }
    }

    private void InitializeEnemiesToSpawn()
    {
        foreach (GameObject enemy in enemyTypes)
        {
            if (enemy.GetComponent<IEnemy>() == null)
            {
                Debug.LogError("Invalid EnemyType");
            }
        }

        enemiesToSpawn = new List<(GameObject, Vector3)>();

        foreach (Vector3 position in enemyPositions)
        {
            System.Random rand = new System.Random();
            int index = rand.Next(enemyTypes.Count);
            enemiesToSpawn.Add((enemyTypes[index], position));
        }
    }

    private Vector3 GetPlayerSpawnPoint(Vector3 roomPos, Vector3 offset)
    {
        Vector3 playerSpawnPoint = (roomPos - (offset * 0.8f));
        playerSpawnPoint.y = 0;
        return playerSpawnPoint;
    }

    public void PlayerEntered(Vector3 doorPos)
    {
        doorPos.y = 0;
        Vector3 direction = (transform.position - doorPos).normalized;
        GameManager.Instance.SetCurrentRoom(transform, direction, this);
        if (roomEntered)
            return;

        roomEntered = true;
        enemySpawner.SpawnEnemies(enemiesToSpawn);
    }

    public void SpawnEnemiesToRoom(List<(GameObject, Vector3)> enemies)
    {
        enemySpawner.SpawnEnemies(enemies);
    }

    public Vector3 GetRoomPosition()
    {
        return transform.position;
    }
}
