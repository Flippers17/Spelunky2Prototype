using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WalkPlayerState : PlayerState
{
    [SerializeField] private float _speed = 4f;

    private float _walkInput = 0;
    
    public override void Awake(){}
    public override void Start(){}
   
    public override void Enter()
    {
        if(_player.isGrounded)
           _anim.SetBool("Walking", true);

        _input.OnAttack += TransitionToAttack;
    }

    public override void UpdateState()
    {
        
        _anim.SetBool("Jumping", !_player.isGrounded);
        
        _walkInput = _input.GetHorizontalMoveInput();
        if (_walkInput != 0)
        {
            _player.facingDirection = _walkInput > 0 ? 1 : -1;
            _player.velocity.x = _speed * _walkInput;
        }
        else
            _player.TransitionToState(_player.idle);

        
    }


    private void CheckTransitions()
    {
        if (LedgeGrabDetection())
            _player.TransitionToState(_player.ledgeGrab);
        else if (_input.RememberJumpInput())
            TransitionToJump();
        else if(_input.HoldingCrouch() && _player.isGrounded)
            _player.TransitionToState(_player.crouching);
        else if (_input.HoldingRun())
            _player.TransitionToState(_player.running);
    }


    private bool LedgeGrabDetection()
    {
        if (_player.isGrounded || _player.velocity.y > 0.1f)
            return false;

        Vector2 playerPos = _player.transform.position;

        if (Physics2D.Raycast(playerPos, Vector2.up, 1.2f, _player.groundMask))
            return false;

        return !Physics2D.Raycast(playerPos + Vector2.up * 0.4f, Vector2.right * _walkInput, 1.2f,
                   _player.groundMask) && Physics2D.Raycast(playerPos + Vector2.up * 0.2f, Vector2.right * _walkInput, 0.7f, _player.groundMask) &&
               !Physics2D.Raycast(playerPos, Vector2.down, 1.5f, _player.groundMask);
    }


    public override void FixedUpdateState()
    {
        if (!_player.isGrounded)
        {
            _anim.SetBool("Walking", false);
            _player.velocity.y -= _player.GetGravity() * Time.fixedDeltaTime;
            
            if(_player.velocity.y < 0)
            {
                Collider2D enemy = Physics2D.OverlapBox(_player.groundCheck.position, _player.groundCheck.localScale, 0, 1 << 9);
                if (enemy != null)
                    if (enemy.TryGetComponent(out EnemyBehaviour eBehaviour))
                    {
                        eBehaviour.TakeDamage(1, Vector2.zero);
                        _player.TransitionToState(_player.jump);
                    }
            }
        }
        else
        {
            _anim.SetBool("Walking", true);
            _player.velocity.y = -1;
        }

        CheckTransitions();
    }

    public override void Exit()
    {
        _anim.SetBool("Walking", false);
        _input.OnAttack -= TransitionToAttack;
    }

    private void TransitionToJump()
    {
        if (!_player.WithinCoyoteTime())
            return;

        _player.TransitionToState(_player.jump);
    }


    private void TransitionToAttack()
    {
        if(!_player.isGrounded)
            return;
        
        _player.TransitionToState(_player.attack);
    }

    public override void OnValidate(PlayerBehaviour player)
    {
        base.OnValidate(player);
    }
    
}
