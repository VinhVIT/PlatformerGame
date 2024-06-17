using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapBoundTrigger : MonoBehaviour
{
    // public static event Action OnRoomChange;
    public Transform player;
    public BoxCollider2D[] roomColliders;
    private BoxCollider2D currentRoomCollider;

    private void Start()
    {

    }
    void Update()
    {
        foreach (BoxCollider2D collider in roomColliders)
        {
            if (collider.OverlapPoint(player.position))
            {
                if (currentRoomCollider != collider)
                {
                    currentRoomCollider = collider;
                    Debug.Log("Player đang ở trong collider: " + currentRoomCollider.name);
                }
                return;
            }
        }
    }
        // void OnTriggerExit2D(Collider2D other)
        // {
        //     if (other.CompareTag("Player"))
        //     {
        //         Debug.Log("Player Leave!");

        //         // OnRoomChange.Invoke();
        //     }
        // }
    }
