using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoom : MonoBehaviour, IRoom
{
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
    BossRoomDoor bossRoomDoorReference;

    [SerializeField]
    EnemySpawner enemySpawnerReference;

    private (Vector3, Quaternion)[] doorOffsets;
    private EnemySpawner enemySpawner;
    private List<(GameObject, Vector3)> enemiesToSpawn;
    private bool roomEntered;

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

        enemySpawner.OnEnemiesActive += (object sender, EnemySpawner.OnEnemiesActiveEventArgs e) =>
        {
            if (roomEntered && !e.active)
            {
                GameManager.Instance.BossKilled();
            }
        };
    }

    private void Start()
    {
        IRoom[] adjantedRooms = GameManager.Instance.GetAdjacedRooms(transform.position);

        for (int i = 0; i < adjantedRooms.Length; i++)
        {
            if (adjantedRooms[i] != null)
            {
                BossRoomDoor door = Instantiate(
                    bossRoomDoorReference,
                    transform.position + doorOffsets[i].Item1,
                    doorOffsets[i].Item2
                );
                door.Setup(adjantedRooms[i], adjantedRooms[i].GetRoomPosition(), enemySpawner);
            }
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

    public void PlayerEntered(Vector3 doorPos)
    {
        MusicManager.Instance.PlayBossMusic();
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
