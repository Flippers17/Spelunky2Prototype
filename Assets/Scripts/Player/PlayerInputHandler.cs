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
    private InputAction crouchAction;
    private InputAction attackAction;
    private InputAction climbAction;
    private InputAction placeBombAction;

    public UnityAction OnJump = () => { };
    public UnityAction OnAttack = () => { };
    public UnityAction OnPlaceBomb = () => { };

    private float _timeSincePressedJump = 1f;
    
    void Start()
    {
        horizontalMoveAction = _input.actions["Move Horizontal"];
        jumpAction = _input.actions["Jump"];
        runAction = _input.actions["Run"];
        crouchAction = _input.actions["Crouch"];
        attackAction = _input.actions["Attack"];
        climbAction = _input.actions["Climb"];
        placeBombAction = _input.actions["Place Bomb"];

        placeBombAction.started += OnPressPlaceBomb;
        jumpAction.started += OnPressJump;
        attackAction.started += OnPressAttack;
    }

    private void OnEnable()
    {
        if (horizontalMoveAction == null)
            return;

        horizontalMoveAction.Enable();
        jumpAction.Enable();
        runAction.Enable();
        crouchAction.Enable();
        attackAction.Enable();
        climbAction.Enable();
    }

    private void OnDisable()
    {
        if (horizontalMoveAction == null)
            return;

        horizontalMoveAction.Disable();
        jumpAction.Disable();
        runAction.Disable();
        crouchAction.Disable();
        attackAction.Disable();
        climbAction.Disable();
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
    
    private void OnPressAttack(InputAction.CallbackContext _)
    {
        OnAttack?.Invoke();
    }
    
    private void OnPressPlaceBomb(InputAction.CallbackContext _)
    {
        OnPlaceBomb?.Invoke();
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
    
    public bool HoldingCrouch()
    {
        return crouchAction.inProgress;
    }
    
    public bool HoldingClimb()
    {
        return climbAction.inProgress;
    }
}
