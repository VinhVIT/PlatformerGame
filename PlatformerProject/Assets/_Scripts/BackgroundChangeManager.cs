using System.Collections.Generic;
using UnityEngine;

public class BackgroundChangeManager : MonoBehaviour
{
    public static BackgroundChangeManager Instance { get; private set; }

    [SerializeField] private List<GameObject> availableBackgrounds;

    private GameObject activeBackground;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        // Initialize the first active background if needed
        if (availableBackgrounds.Count > 0)
        {
            activeBackground = availableBackgrounds[0];
            activeBackground.SetActive(true);
        }
    }

    public void ChangeBackground(GameObject newBackground)
    {
        if (activeBackground != null)
        {
            activeBackground.SetActive(false);
        }

        activeBackground = newBackground;
        activeBackground.SetActive(true);
    }

    public GameObject GetActiveBackground()
    {
        return activeBackground;
    }
}

