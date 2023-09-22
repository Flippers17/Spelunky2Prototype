using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField]
    private HurtBox _hurtBox;

    private Transform pickupTarget;
    [SerializeField]
    private Rigidbody2D _rb;
    [SerializeField]
    private bool _destroyOnHit = true;

    [SerializeField]
    private LayerMask _hitMask;

    private bool isThrown = false;

    private void OnValidate()
    {
        if (!_rb)
            if (!TryGetComponent(out _rb))
                Debug.LogWarning("Item is missing Rigidbody2D reference", this);
    }

    public void PickUp(Transform target)
    {
        _rb.isKinematic = true;
        pickupTarget = target;
    }

    private void Update()
    {
        if(pickupTarget != null)
        {
            transform.position = Vector3.Lerp(transform.position, pickupTarget.position, 0.1f);
        }
    }

    public void ThrowItem(Vector2 velocity)
    {
        pickupTarget = null;
        _rb.isKinematic = false;
        isThrown = true;
        _rb.velocity = velocity;
    }
    
    public void PlaceItemDown()
    {
        pickupTarget = null;
        _rb.isKinematic = false;
        isThrown = false;
        _rb.velocity = new Vector2(0, -3);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(!isThrown)
        {
            return;
        }

        if((1 << collision.gameObject.layer & _hitMask) != 0)
        {
            OnHit();
        }   
    }

    protected virtual void OnHit()
    {
        if(_hurtBox)
            _hurtBox.enabled = true;
        if(_destroyOnHit)
            Destroy(gameObject);
    }
}
