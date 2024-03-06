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
        player.AttackState.AddToDetected(collision);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {   

        player.AttackState.RemoveFromDetected(collision);
    }
}
