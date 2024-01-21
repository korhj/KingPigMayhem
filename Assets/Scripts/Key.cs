using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    [SerializeField]
    GameObject keyVisual;

    private void FixedUpdate() { }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject == Player.Instance.gameObject)
        {
            Destroy(keyVisual);
            Player.Instance.CollectKey();
        }
    }
}
