using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    public int currentHealth = 4;
    public GameObject deathParticle;
    
    public UnityAction OnDie;

    private void OnEnable()
    {
        OnDie += Dying;
    }

    private void OnDisable()
    {
        OnDie -= Dying;
    }

    public virtual void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
            OnDie?.Invoke();
    }

    private void Dying()
    {
        Instantiate(deathParticle, transform.position, transform.rotation);
    }
}
