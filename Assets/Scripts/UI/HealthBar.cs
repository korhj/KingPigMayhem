using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        Player.Instance.OnHealthUpdate += OnPlayerHealthUpdate;
    }

    private void OnPlayerHealthUpdate(object sender,  Player.OnHealthUpdateEventArgs e ) {
        Debug.Log("Health Update" + e.playerCurrentHealth);
    }

}
