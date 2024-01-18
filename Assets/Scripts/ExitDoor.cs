using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Drawing;
using UnityEngine;

public class ExitDoor : MonoBehaviour
{
    public event EventHandler OnEnter;

    private bool isActive;

    void Start()
    {
        isActive = false;
    }

    public void Setup(EnemySpawner enemySpawner)
    {
        enemySpawner.OnEnemiesDead += (object sender, EventArgs e) =>
        {
            Debug.Log(gameObject.GetInstanceID());
            isActive = true;
        };
    }

    private void OnTriggerEnter(Collider collider)
    {
        Debug.Log("ExitDoor trigger " + isActive);
        if (collider.gameObject == Player.Instance.gameObject & Player.Instance.KeyCollected())
        {
            Debug.Log("Victory!");
            Debug.Log(gameObject.GetInstanceID());
            OnEnter?.Invoke(this, EventArgs.Empty);
        }
    }
}
