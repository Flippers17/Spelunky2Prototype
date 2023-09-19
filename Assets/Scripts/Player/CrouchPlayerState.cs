using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CrouchPlayerState : PlayerState
{
    [SerializeField] private float _speed = 2f;
    [SerializeField] private Collider2D _defaultCollider;
    [SerializeField] private Collider2D _crouchCollider;

    private float _walkInput = 0;
    
    public override void Awake(){}
    public override void Start(){}
   
    public override void Enter()
    {
        _defaultCollider.enabled = false;
        _crouchCollider.enabled = true;
    }

    public override void UpdateState()
    {
        _walkInput = _input.GetHorizontalMoveInput();
        if (_walkInput != 0)
        {
            _player.facingDirection = _walkInput > 0 ? 1 : -1;
            _player.velocity.x = _speed * _walkInput;
        }
        else
        {
            _player.velocity.x = 0;
        }

        CheckTransitions();
    }


    private void CheckTransitions()
    {
        if(CeilingDetection())
            return;
        Debug.Log("Here");
        
        if(!_input.HoldingCrouch() || !_player.isGrounded)
            _player.TransitionToState(_player.idle);
        else if (_input.RememberJumpInput())
            TransitionToJump();
    }


    private bool CeilingDetection()
    {
        return Physics2D.OverlapBox((Vector2)_player.transform.position + Vector2.up * 0.25f, new Vector2(1.05f, 0.6f), 0,
            _player._groundMask);
    }
    

    public override void FixedUpdateState()
    {
        if (!_player.isGrounded)
            _player.velocity.y -= _player.GetGravity() * Time.fixedDeltaTime;
        else
            _player.velocity.y = -1;
    }

    public override void Exit()
    {
        _defaultCollider.enabled = true;
        _crouchCollider.enabled = false;
    }

    private void TransitionToJump()
    {
        if (!_player.WithinCoyoteTime())
            return;

        _player.TransitionToState(_player.jump);
    }

    public override void OnValidate(PlayerBehaviour player)
    {
        base.OnValidate(player);
    }
    
}
