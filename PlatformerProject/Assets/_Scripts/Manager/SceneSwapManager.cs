using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class SceneSwapManager : MonoBehaviour
{
    public static SceneSwapManager Instance;
    private static bool loadFromMap;
    private GameObject player;
    private Transform mapSpawnPoint;
    private Vector3 playerSpawnPosition;
    private MapChangeTrigger.MapTrigger directionToSpawn;
    private void Awake()
    {
        if (Instance == null) Instance = this;
        player = GameObject.FindGameObjectWithTag("Player");
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoad;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoad;
    }
    public static void SwapSceneFromMapUse(SceneField myScene, MapChangeTrigger.MapTrigger directionToSpawn)
    {
        loadFromMap = true;
        Instance.StartCoroutine(Instance.FadeOutThenChangeScene(myScene, directionToSpawn));
    }
    private IEnumerator FadeOutThenChangeScene(SceneField myScene, MapChangeTrigger.MapTrigger directionToSpawn = MapChangeTrigger.MapTrigger.None)
    {
        PlayerInputHandler.DeactivatePlayerControl();
        SceneFadeManager.Instance.StartFadeOut();

        while (SceneFadeManager.Instance.IsFadingOut)
        {
            yield return null;
        }

        this.directionToSpawn = directionToSpawn;
        SceneManager.LoadScene(myScene);
    }
    private IEnumerator ActivatePlayerControlAfterFadeIn()
    {
        while (SceneFadeManager.Instance.IsFadingIn)
        {
            yield return null;
        }
        PlayerInputHandler.ActivatePlayerControl();
    }
    private void OnSceneLoad(Scene scene, LoadSceneMode mode)
    {
        SceneFadeManager.Instance.StartFadeIn();

        if (loadFromMap)
        {
            StartCoroutine(ActivatePlayerControlAfterFadeIn());

            FindDirectionToSpawn(directionToSpawn);
            player.transform.position = playerSpawnPosition;
            loadFromMap = false;
        }
    }
    private void FindDirectionToSpawn(MapChangeTrigger.MapTrigger directionToSpawn)
    {
        MapChangeTrigger[] mapChangeTriggers = FindObjectsOfType<MapChangeTrigger>();
        for (int i = 0; i < mapChangeTriggers.Length; i++)
        {
            if (mapChangeTriggers[i].currentMapTriggerDir == directionToSpawn)
            {
                mapSpawnPoint = mapChangeTriggers[i].gameObject.transform.GetChild(0);

                playerSpawnPosition = mapSpawnPoint.position;

                return;
            }
        }
    }
}
