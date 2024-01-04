using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Petrifex : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float petrifexHeight = .7f;
    [SerializeField] private float petrifexRadius = 2f;
    [SerializeField] private float petrifexPosition = 5f;
   
    private void Start() {
    }

    private void Update() {

        Vector3 moveDir = Player.Instance.GetPlayerPosition() - transform.position;
        float moveDistance = moveSpeed * Time.deltaTime;
        transform.position += moveDir * moveDistance;
        
        /*
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * petrifexHeight , petrifexRadius, moveDir, moveDistance);
        if (!canMove) {
            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0);
            canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * petrifexHeight , petrifexRadius, moveDirX, moveDistance);
            if (canMove) {
                moveDir = moveDirX.normalized;
            } else {
                Vector3 moveDirZ = new Vector3(0, 0, moveDir.z);
                canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * petrifexHeight , petrifexRadius, moveDirZ, moveDistance);
                if (canMove) {
                    moveDir = moveDirZ.normalized;
                }
            }

        }
        if (canMove) {
            transform.position += moveDir * moveDistance;
        }
        */
    }
        
}
