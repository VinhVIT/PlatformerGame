using System;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class AreaTrigger : MonoBehaviour
{
    public static event Action<Collider2D, float> OnAreaChange;
    private Transform destination;
    private bool isChangeArea = false;
    private float delayTime = 0.5f;// THIS VALUE DECIDE HOW LONG THE CHANGE START
    [SerializeField] private Collider2D nextAreaBound;
    [SerializeField] public bool needChangeBackground;
    [HideInInspector] public GameObject backgroundToChange;

    private void Start()
    {
        destination = transform.GetChild(0);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // move to next area
            OnAreaChange?.Invoke(nextAreaBound, delayTime);
            StartCoroutine(ChangePosition(other, destination));
            //change background
            if (needChangeBackground && backgroundToChange != null)
            {
                StartCoroutine(ChangeBackground());
            }
        }
    }
    private IEnumerator ChangePosition(Collider2D other, Transform pos)
    {
        float transitionWaitingTime = delayTime * 2;// X2 because each transition have start and end
        PlayerInput playerInput = other.GetComponent<PlayerInput>();
        
        playerInput.enabled = false;
        Time.timeScale = 0f;

        yield return new WaitForSecondsRealtime(delayTime);
        //turn off player input and move player
        other.transform.position = pos.position;
        Time.timeScale = 1f;
        isChangeArea = !isChangeArea;

        yield return new WaitForSeconds(transitionWaitingTime);
        //turn on player input
        playerInput.enabled = true;
    }

    private IEnumerator ChangeBackground()
    {
        yield return new WaitForSeconds(delayTime);
        BackgroundChangeManager.Instance.ChangeBackground(backgroundToChange);
    }
}
[CustomEditor(typeof(AreaTrigger))]
public class AreaTriggerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        AreaTrigger areaTrigger = (AreaTrigger)target;

        DrawDefaultInspector();

        // if needChangeBackground ticked show backgroundTOChange
        if (areaTrigger.needChangeBackground)
        {
            areaTrigger.backgroundToChange = (GameObject)EditorGUILayout.ObjectField
            ("Background To Change", areaTrigger.backgroundToChange, typeof(GameObject), true);
        }

        // saveEditor
        if (GUI.changed)
        {
            EditorUtility.SetDirty(areaTrigger);
        }
    }
}