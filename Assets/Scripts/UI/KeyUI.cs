using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyUI : MonoBehaviour
{
    void Start()
    {
        gameObject.SetActive(false);
        Player.Instance.OnKeyPickup += KeyUI_OnKeyPickup;
    }

    private void KeyUI_OnKeyPickup(object sender, EventArgs e)
    {
        Debug.Log("Key picked up");
        gameObject.SetActive(true);
    }
}
