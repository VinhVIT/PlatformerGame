using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackHitbox : MonoBehaviour
{
    private Player player;

    private void Awake()
    {
        player = GetComponentInParent<Player>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        player.GroundAttackState.AddToDetected(collision);
        player.AirAttackState.AddToDetected(collision);
        player.BlockCounterState.AddToDetected(collision);
        player.HolySlashState.AddToDetected(collision);
        player.LightCutAttackState.AddToDetected(collision);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        player.GroundAttackState.RemoveFromDetected(collision);
        player.AirAttackState.RemoveFromDetected(collision);
        player.BlockCounterState.RemoveFromDetected(collision);
        player.HolySlashState.RemoveFromDetected(collision);
        player.LightCutAttackState.RemoveFromDetected(collision);

    }
}
