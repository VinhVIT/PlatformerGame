using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Stat
{
    public event Action OnCurrentValueZero;

    [field: SerializeField] public int MaxValue { get; private set; }

    public int CurrentValue
    {
        get => currentValue;
        set
        {
            currentValue = Mathf.Clamp(value, 0, MaxValue);

            if (currentValue <= 0)
            {
                OnCurrentValueZero?.Invoke();
            }
        }
    }

    private int currentValue;
    public float LastDecreaseTime { get; private set; }
    public float LastIncreaseTime { get; private set; }

    public void Init() => CurrentValue = MaxValue;

    public void Increase(int amount) => CurrentValue += amount;

    public void Decrease(int amount) => CurrentValue -= amount;
    public void ContinuousDecrease(int amount, float decreaseRate)
    {
        if (Time.time >= LastDecreaseTime + decreaseRate)
        {
            Decrease(amount);
            LastDecreaseTime = Time.time;
        }
    }
    public void ContinuousIncrease(int amount, float increaseRate)
    {
        if (Time.time >= LastIncreaseTime + increaseRate)
        {
            Increase(amount);
            LastIncreaseTime = Time.time;
        }
    }
    public bool EnoughToUse(int amount)
    {
        if (CurrentValue >= amount)
        {
            return true;
        }
        Debug.Log("Not enough Value to use");
        return false;
    }
}
