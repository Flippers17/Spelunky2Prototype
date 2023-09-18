using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    [SerializeField] private PlayerInput _input;
    [SerializeField]
    private float _jumpPressRememberTime = 0.1f;

    private InputAction horizontalMoveAction;
    private InputAction jumpAction;
    private InputAction runAction;

    public UnityAction OnJump = () => { };

    private float _timeSincePressedJump = 1f;
    
    void Start()
    {
        horizontalMoveAction = _input.actions["Move Horizontal"];
        jumpAction = _input.actions["Jump"];
        runAction = _input.actions["Run"];
        jumpAction.started += OnPressJump;
    }

    private void Update()
    {
        if (_timeSincePressedJump < _jumpPressRememberTime)
            _timeSincePressedJump += Time.deltaTime;
    }

    public float GetHorizontalMoveInput()
    {
        return horizontalMoveAction.ReadValue<float>();
    }

    private void OnPressJump(InputAction.CallbackContext _)
    {
        _timeSincePressedJump = 0;
        OnJump?.Invoke();
    }


    public bool RememberJumpInput()
    {
        return _timeSincePressedJump < _jumpPressRememberTime;
    }

    public bool HoldingJump()
    {
        return jumpAction.inProgress;
    }
    
    public bool HoldingRun()
    {
        return runAction.inProgress;
    }
}
