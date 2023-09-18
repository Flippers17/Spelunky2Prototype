using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    [SerializeField] private PlayerInput _input;

    private InputAction horizontalMoveAction;
    private InputAction jumpAction;

    public UnityAction OnJump = () => { };
    
    void Start()
    {
        horizontalMoveAction = _input.actions["Move Horizontal"];
        jumpAction = _input.actions["Jump"];
        jumpAction.started += OnPressJump;
    }

    public float GetHorizontalMoveInput()
    {
        return horizontalMoveAction.ReadValue<float>();
    }

    private void OnPressJump(InputAction.CallbackContext _)
    {
        OnJump?.Invoke();
    }

    public bool HoldingJump()
    {
        return jumpAction.inProgress;
    }
    
}
