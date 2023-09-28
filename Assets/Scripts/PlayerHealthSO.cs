using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Player Health SO", fileName = "Player Health")]
public class PlayerHealthSO : ScriptableObject
{
    public int startHealth = 4;
    public int currentHealth = 4;

    public UnityAction<int> OnChangeHealth = delegate { };
    public UnityAction OnDie = delegate { };

    public void ResetHealth()
    {
        currentHealth = startHealth;
        OnChangeHealth?.Invoke(currentHealth);
    }

    public void ChangeHealth(int amount)
    {
        currentHealth += amount;
        OnChangeHealth?.Invoke(currentHealth);
        if (currentHealth <= 0)
            Die();
    }

    public void Die()
    {
        OnDie?.Invoke();
    }
}
