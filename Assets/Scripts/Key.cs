using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    private void FixedUpdate() { }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject == Player.Instance.gameObject)
        {
            Destroy(gameObject);
            Player.Instance.CollectKey();
        }
    }
}
