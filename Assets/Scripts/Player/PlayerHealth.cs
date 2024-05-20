using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : Health
{
    [SerializeField]
    private PlayerHealthSO phSO;

    private void Start()
    {
        currentHealth = phSO.currentHealth;
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        phSO.ChangeHealth(-damage);
    }

}
