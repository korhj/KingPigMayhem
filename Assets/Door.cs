using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using UnityEngine;

public class Door : MonoBehaviour {

    private bool isActive;
    void Start() {
        isActive = false;
    }

    private void OnTriggerEnter(Collider collider) {
        if(isActive) {
           Debug.Log("Entered a door");
        }
        
    }
}
