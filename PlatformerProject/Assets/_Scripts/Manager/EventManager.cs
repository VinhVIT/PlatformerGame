using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class EventManager
{
    public static readonly PlayerEvents Player = new PlayerEvents();
    public static readonly TriggerEvents Trigger = new TriggerEvents();

    public class PlayerEvents
    {
        public UnityAction OnSpellCastDone;
        public UnityAction OnZeroHealth;
    }
    public class TriggerEvents
    {
        public UnityAction<float, Collider2D> OnAreaChange;
        public UnityAction<Collider2D> OnMapChange;
    }
}
