using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour, IDamageable
{
    [SerializeField] private Health _health;
    [SerializeField] private float _invincibillityTime = 0.2f;
    private float _timeSinceHit = 0;


    private void OnValidate()
    {
        if(_health == null)
            if(!TryGetComponent(out _health))
                Debug.LogWarning("EnemyBehaviour is missing Health reference", this);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnEnable()
    {
        _health.OnDie += Die;
    }

    private void OnDisable()
    {
        _health.OnDie -= Die;
    }

    // Update is called once per frame
    void Update()
    {
        if (_timeSinceHit < _invincibillityTime)
            _timeSinceHit += Time.deltaTime;
    }

    public void TakeDamage(int damage, Vector2 knockback)
    {
        _timeSinceHit = 0;
        _health.TakeDamage(damage);
    }

    public void Die()
    {
        Debug.Log(this.gameObject.name + " has died");
        Destroy(gameObject);
    }

    public bool CanBeHit()
    {
        return _timeSinceHit > _invincibillityTime;
    }
}
