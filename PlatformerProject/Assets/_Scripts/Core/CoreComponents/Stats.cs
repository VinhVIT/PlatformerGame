using System;
using UnityEngine;
[Serializable]
public class Stats : CoreComponent
{
    [field: SerializeField] public Stat Health { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        Health.Init();
    }

}

