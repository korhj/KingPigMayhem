using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Transform petrifex;
    [SerializeField] private Transform frog;
    void Start()
    {
        Instantiate(petrifex, new Vector3(-5, 0, 0), UnityEngine.Quaternion.identity);
        Instantiate(petrifex, new Vector3(-5, 0, 10), UnityEngine.Quaternion.identity);
        Instantiate(frog, new Vector3(5, 0, 10), UnityEngine.Quaternion.identity);
    }

}
