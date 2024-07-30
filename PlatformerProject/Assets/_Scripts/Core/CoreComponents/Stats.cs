using System;
using UnityEngine;
[Serializable]
public class Stats : CoreComponent
{
    public event Action OnHealthZero;

    [SerializeField] private int maxHealth;
    [SerializeField] private int currentHealth;

    protected override void Awake()
    {
        base.Awake();

        currentHealth = maxHealth;
    }

    public void DecreaseHealth(int amount)
    {
        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            currentHealth = 0;

            OnHealthZero?.Invoke();

            Debug.Log("Health is zero!!");
        }
    }

    public void IncreaseHealth(int amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
    }
}

