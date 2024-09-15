using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;
    private List<CinemachineVirtualCamera> allVirtualCameras = new List<CinemachineVirtualCamera>();
    [Header("Controls for lerping the Y Damping during player jump/fall")]
    [SerializeField] private float _fallPanAmount = 0.25f;
    [SerializeField] private float _fallYPanTime = 0.35f;
    public float fallSpeedYDampingChangeThreshold = -15f;
    public bool IsLerpingYDamping { get; private set; }
    public bool LerpedFromPlayerFalling { get; set; }
    private Coroutine _lerpYPanCoroutine;
    private Coroutine _panCameraCorountine;
    private CinemachineVirtualCamera _currentCamera;
    private CinemachineFramingTransposer _framingTransposer;
    private float _normYPanAmount;
    private Vector2 _startingTrackedObjectOffset;
    private CinemachineConfiner2D _confiner;

    [Space]
    [Header("Screen Shake")]
    [SerializeField] private float shakeForce = 1f;
    private CinemachineImpulseDefinition impulseDefinition;
    private CinemachineImpulseListener impulseListener;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        FindAllCamerasInScene();
        //set YDaming so it base on Inspector value
        _normYPanAmount = _framingTransposer.m_YDamping;

        //set starting pos of the tracked object offset
        _startingTrackedObjectOffset = _framingTransposer.m_TrackedObjectOffset;
    }
    private void FindAllCamerasInScene()
    {
        // Find all virtual cameras in the current scene and add them to the list
        allVirtualCameras.AddRange(FindObjectsOfType<CinemachineVirtualCamera>());

        // Set the initial current camera, framing transposer, and confiner
        foreach (CinemachineVirtualCamera vcam in allVirtualCameras)
        {
            if (vcam.enabled)
            {
                _currentCamera = vcam;
                _framingTransposer = _currentCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
                _confiner = _currentCamera.GetComponent<CinemachineConfiner2D>();
                break;
            }
        }
    }
    private void Start()
    {
        EventManager.Trigger.OnMapChange += AreaTrigger_OnMapChange;

    }

    private void AreaTrigger_OnMapChange(Collider2D mapBound)
    {
        _confiner.m_BoundingShape2D = mapBound;
    }

    #region Lerp the Y Damping
    public void LerpYDamping(bool isPlayerFalling)
    {
        _lerpYPanCoroutine = StartCoroutine(LerpYAction(isPlayerFalling));
    }
    private IEnumerator LerpYAction(bool isPlayerFalling)
    {
        IsLerpingYDamping = true;
        //grab the starting damping amount
        float startDampAmount = _framingTransposer.m_YDamping;
        float endDampAmount = 0f;
        //determine the end damping amount
        if (isPlayerFalling)
        {
            endDampAmount = _fallPanAmount;
            LerpedFromPlayerFalling = true;
        }
        else
        {
            endDampAmount = _normYPanAmount;
        }
        //lerp the pan amount
        float elapsedTime = 0f;
        while (elapsedTime < _fallYPanTime)
        {
            elapsedTime += Time.deltaTime;

            float lerpedPanAmount = Mathf.Lerp(startDampAmount, endDampAmount, elapsedTime / _fallYPanTime);
            _framingTransposer.m_YDamping = lerpedPanAmount;
            yield return null;
        }
        IsLerpingYDamping = false;
    }
    #endregion
    #region Pan Camera
    public void PanCameraOnContact(float panDistance, float panTime, PanDirection panDirection, bool panToStartingPos)
    {
        _panCameraCorountine = StartCoroutine(PanCamera(panDistance, panTime, panDirection, panToStartingPos));
    }
    private IEnumerator PanCamera(float panDistance, float panTime, PanDirection panDirection, bool panToStartingPos)
    {
        Vector2 endPos = Vector2.zero;
        Vector2 startingPos = Vector2.zero;
        //handle pan from trigger
        if (!panToStartingPos)
        {
            switch (panDirection)
            {
                case PanDirection.Up:
                    endPos = Vector2.up;
                    break;
                case PanDirection.Down:
                    endPos = Vector2.down;
                    break;
                case PanDirection.Left:
                    endPos = Vector2.right;
                    break;
                case PanDirection.Right:
                    endPos = Vector2.left;
                    break;
                default:
                    break;
            }
            endPos *= panDistance;
            startingPos = _startingTrackedObjectOffset;
            endPos += startingPos;
        }
        //handle the direction settings when back to starting pos
        else
        {
            startingPos = _framingTransposer.m_TrackedObjectOffset;
            endPos = _startingTrackedObjectOffset;
        }
        //handle the actual pan of cam
        float elapsedTime = 0f;
        while (elapsedTime <= panTime)
        {
            elapsedTime += Time.deltaTime;

            Vector3 panLerp = Vector3.Lerp(startingPos, endPos, elapsedTime / panTime);
            _framingTransposer.m_TrackedObjectOffset = panLerp;
            yield return null;
        }
    }
    #endregion
    #region  Swap Cameras
    public void SwapCamera(CinemachineVirtualCamera cameraFromLeft, CinemachineVirtualCamera cameraFromRight, Vector2 triggerExitDirection)
    {
        //if the current camera is the camera from the left and our trigger exit direction was on the right
        if (_currentCamera == cameraFromLeft && triggerExitDirection.x > 0f)
        {
            //activate new cam
            cameraFromRight.enabled = true;
            //deactivate old cam
            cameraFromLeft.enabled = false;
            //set new cam as the current cam
            _currentCamera = cameraFromRight;
            //update composer variable
            _framingTransposer = _currentCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        }
        //if the current camera is the camera from the right and our trigger exit direction was on the left
        else if (_currentCamera == cameraFromRight && triggerExitDirection.x < 0f)
        {
            //activate new cam
            cameraFromLeft.enabled = true;
            //deactivate old cam
            cameraFromRight.enabled = false;
            //set new cam as the current cam
            _currentCamera = cameraFromLeft;
            //update composer variable
            _framingTransposer = _currentCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        }
    }
    #endregion
    #region Shake Camera
    public void Shake(CinemachineImpulseSource impulseSource)
    {
        if (impulseSource == null)
        {
            Debug.Log("impulseSource null");
        }
        impulseSource.GenerateImpulseWithForce(shakeForce);
    }
    public void ShakeWithProfile(ScreenShakeProfile profile,CinemachineImpulseSource impulseSource)
    {
        if (impulseSource == null)
        {
            Debug.Log("impulseSource null");
        }
        impulseDefinition = impulseSource.m_ImpulseDefinition;

        impulseSource.m_ImpulseDefinition.m_ImpulseShape = profile.impulseShape;
        impulseDefinition.m_ImpulseDuration = profile.impactTime;
        impulseSource.m_DefaultVelocity = profile.defaultVelocity;

        impulseListener = _currentCamera.GetComponent<CinemachineImpulseListener>();
        impulseListener.m_ReactionSettings.m_AmplitudeGain = profile.listenerAmplitude;
        impulseListener.m_ReactionSettings.m_FrequencyGain = profile.listenerFrequency;
        impulseListener.m_ReactionSettings.m_Duration = profile.listenerDuration;


        impulseSource.GenerateImpulseWithForce(profile.impactForce);
    }
    #endregion
}