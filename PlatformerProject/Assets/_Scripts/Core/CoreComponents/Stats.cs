using System;
using UnityEngine;

public class Stats : CoreComponent
{

    [SerializeField] private float maxHealth = 50f;
    private float currentHealth;

    protected override void Awake()
    {
        base.Awake();

        currentHealth = maxHealth;
    }
    public void DecreaseHealth(float amount)
    {
        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            currentHealth = 0f;
            EventManager.Player.OnZeroHealth?.Invoke();
        }
    }
    public void IncreaseHealth(float amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
    }
}
