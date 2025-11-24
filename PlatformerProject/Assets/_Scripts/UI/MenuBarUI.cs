using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuBarUI : MonoBehaviour
{
    [SerializeField] private GameObject containerBar;
    [SerializeField] private GameObject[] MenuItems;
    private Transform[] barItems;
    private int currentItem = 0;

    private void Awake()
    {
        // Get all item in container
        int childCount = containerBar.transform.childCount;

        barItems = new Transform[childCount];
        for (int i = 0; i < childCount; i++)
        {
            barItems[i] = containerBar.transform.GetChild(i).transform;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            MoveToPreviousItem();
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            MoveToNextItem();
        }
        UpdateItemAnimations();
    }

    private void MoveToPreviousItem()
    {
        currentItem = (currentItem - 1 + barItems.Length) % barItems.Length;
    }

    private void MoveToNextItem()
    {
        currentItem = (currentItem + 1) % barItems.Length;
    }

    private void UpdateItemAnimations()
    {
        for (int i = 0; i < barItems.Length; i++)
        {
            Animator anim = barItems[i].GetComponent<Animator>();
            if (anim != null)
            {
                if (i == currentItem)
                {
                    anim.SetBool("choosing", true);
                    MenuItems[i].SetActive(true);
                }
                else
                {
                    anim.SetBool("choosing", false);
                    MenuItems[i].SetActive(false);
                }
            }
        }
    }
}
