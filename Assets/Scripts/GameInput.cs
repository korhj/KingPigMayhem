using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInput : MonoBehaviour
{
    private PlayerInputActions playerInputActions;

    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();
    }

    public Vector2 GetMovementVectorNormalized()
    {
        Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();
        return inputVector.normalized;
    }

    public Vector2 GetAimVectorNormalized()
    {
        Vector2 inputVector = playerInputActions.Player.Aim.ReadValue<Vector2>();
        return inputVector.normalized;
    }

    public Vector2 GetChargeAttackVectorNormalized()
    {
        Vector2 inputVector = playerInputActions.Player.ChargeAttack.ReadValue<Vector2>();
        return inputVector.normalized;
    }
}
