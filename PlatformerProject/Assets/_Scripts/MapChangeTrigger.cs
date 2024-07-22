using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapChangeTrigger : MonoBehaviour
{
    public enum MapTrigger
    {
        None,
        Down,
        Up,
        Left,
        Right
    }
    [Header("Spawn TO")]
    [SerializeField] private MapTrigger nextMapTriggerDir;
    [SerializeField] private SceneField sceneToLoad;
    [SerializeField] private Collider2D nextMapBound;
    [Header("This Map")]
    public MapTrigger currentMapTriggerDir;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            ChangeMap();
        }
    }
    private void ChangeMap()
    {
        EventManager.Trigger.OnMapChange?.Invoke(nextMapBound);
        SceneSwapManager.SwapSceneFromMapUse(sceneToLoad, nextMapTriggerDir);
    }
}
