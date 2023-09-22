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
        _anim.SetBool("Crawling", true);
        _defaultCollider.enabled = false;
        _crouchCollider.enabled = true;
        _input.OnAttack += PickUpItem;
    }

    public override void UpdateState()
    {
        _walkInput = _input.GetHorizontalMoveInput();
        if (_walkInput != 0)
        {
            _player.facingDirection = _walkInput > 0 ? 1 : -1;
            _player.velocity.x = _speed * _walkInput;
            _anim.speed = 1;
        }
        else
        {
            _player.velocity.x = 0;
            _anim.speed = 0;
        }

        CheckTransitions();
    }


    private void CheckTransitions()
    {
        if (LedgeGrabDetection() && _input.HoldingCrouch() && _input.HoldingJump())
        {
            _player.TransitionToState(_player.climbDown);
            return;
        }

        if(CeilingDetection())
            return;
        
        if(!_input.HoldingCrouch() || !_player.isGrounded)
        {
            _player.TransitionToState(_player.idle);
            return;
        }
        else if (_input.RememberJumpInput())
            TransitionToJump();
    }


    private bool CeilingDetection()
    {
        return Physics2D.OverlapBox((Vector2)_player.transform.position + Vector2.up * 0.25f, new Vector2(1.05f, 0.6f), 0,
            _player._groundMask);
    }

    private bool LedgeGrabDetection()
    {
        if (!_player.isGrounded)
            return false;

        Vector2 playerPos = _player.transform.position;

        return !Physics2D.Raycast(playerPos + Vector2.right * (.8f * _player.facingDirection), Vector2.down, 2f, _player._groundMask)
                && _player.isGrounded;
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
        _anim.SetBool("Crawling", false);
        _anim.speed = 1;
        _input.OnAttack -= PickUpItem;
    }

    private void TransitionToJump()
    {
        if (!_player.WithinCoyoteTime())
            return;

        _player.TransitionToState(_player.jump);
    }


    private void PickUpItem()
    {
        if(_player.currentHeldItem != null)
        {
            _player.currentHeldItem.PlaceItemDown();
            _player.currentHeldItem = null;
            return;
        }

        Collider2D[] items = Physics2D.OverlapCircleAll(_player.transform.position + Vector3.right * _player.facingDirection, 0.5f, 1 << 10);

        if (items == null)
            return;

        if (!items[0].TryGetComponent(out Item currentItem))
            return;

        currentItem.PickUp(_player._itemPickupPoint);
        _player.currentHeldItem = currentItem;
    }

    public override void OnValidate(PlayerBehaviour player)
    {
        base.OnValidate(player);
    }
    
}
