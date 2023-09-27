using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour, IDamageable
{
    [SerializeField]
    private HurtBox _hurtBox;
    [SerializeField]
    protected Health _health;

    private Transform pickupTarget;
    [SerializeField]
    private Rigidbody2D _rb;
    [SerializeField]
    private int hitDamageTaken = 1;

    [SerializeField]
    private LayerMask _hitMask;

    private bool isThrown = false;

    protected virtual void OnValidate()
    {
        if (!_rb)
            if (!TryGetComponent(out _rb))
                Debug.LogWarning("Item is missing Rigidbody2D reference", this);
        
        if (!_health)
            if (!TryGetComponent(out _health))
                Debug.LogWarning("Item is missing Health reference", this);
    }

    protected virtual void OnEnable()
    {
        _hurtBox.OnHit += OnHit;
        _health.OnDie += Die;
    }

    protected virtual void OnDisable()
    {
        _hurtBox.OnHit -= OnHit;
        _health.OnDie -= Die;
    }

    public void PickUp(Transform target)
    {
        _rb.isKinematic = true;
        pickupTarget = target;
    }

    protected virtual void Update()
    {
        if(pickupTarget != null)
        {
            transform.position = Vector3.Lerp(transform.position, pickupTarget.position, 0.1f);
        }
    }

    public virtual void ThrowItem(Vector2 velocity)
    {
        pickupTarget = null;
        _rb.isKinematic = false;
        _hurtBox.enabled = true;
        isThrown = true;
        _rb.velocity = velocity;
    }
    
    public virtual void PlaceItemDown()
    {
        pickupTarget = null;
        _rb.isKinematic = false;
        _hurtBox.enabled = false;
        isThrown = false;
        _rb.velocity = new Vector2(0, -3);
    }


    public void TakeDamage(int dmg, Vector2 knockback)
    {
        _health.TakeDamage(dmg);
    }

    public virtual void Die()
    {
        Destroy(gameObject);
    }

    public bool CanBeHit()
    {
        return true;
    }

   
    protected virtual void OnHit()
    {
        _rb.velocity = Vector2.zero;
        TakeDamage(hitDamageTaken, Vector2.zero);
    }
}
