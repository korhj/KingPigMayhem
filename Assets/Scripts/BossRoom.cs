using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoom : MonoBehaviour, IRoom
{
    [SerializeField]
    Transform kingPig;

    [SerializeField]
    private float doorXoffset;

    [SerializeField]
    private float doorYffset;

    [SerializeField]
    private float doorZffset;

    [SerializeField]
    BossRoomDoor bossRoomDoorReference;

    [SerializeField]
    BossSpawner bossSpawnerReference;

    private (Vector3, Quaternion)[] doorOffsets;
    private ISpawner bossSpawner;
    private bool roomEntered;

    private void Awake()
    {
        bossSpawner = Instantiate(bossSpawnerReference, transform.position, Quaternion.identity);

        roomEntered = false;

        doorOffsets = new (Vector3 Position, Quaternion Rotation)[]
        {
            (new Vector3(0, doorYffset, doorZffset), Quaternion.Euler(0, 0, 0)), // Up: Position and Rotation
            (new Vector3(0, doorYffset, -doorZffset), Quaternion.Euler(0, 0, 0)), // Down: Position and Rotation
            (new Vector3(-doorXoffset, doorYffset, 0), Quaternion.Euler(0, 90, 0)), // Left: Position and Rotation
            (new Vector3(doorXoffset, doorYffset, 0), Quaternion.Euler(0, -90, 0)) // Right: Position and Rotation
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
                door.Setup(adjantedRooms[i], adjantedRooms[i].GetRoomPosition(), bossSpawner);
            }
        }
    }

    public void PlayerEntered(Vector3 doorPos)
    {
        doorPos.y = 0;
        Vector3 direction = (transform.position - doorPos).normalized;
        GameManager.Instance.SetCurrentRoomTransform(transform, direction);
        if (roomEntered)
            return;

        roomEntered = true;
        bossSpawner.SpawnEnemies();
    }

    public Vector3 GetRoomPosition()
    {
        return transform.position;
    }
}
