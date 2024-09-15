using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "ScreenShake/New Profile")]
public class ScreenShakeProfile : ScriptableObject
{
    [Header("Impulse Source Settings")]
    public Cinemachine.CinemachineImpulseDefinition.ImpulseShapes impulseShape;
    public float impactTime = .2f;
    public float impactForce = 1f;
    public Vector3 defaultVelocity = new Vector3(0f, -1f, 0f);
    // public AnimationCurve animationCurve;
    [Header("Impulse Listener Settings")]
    public float listenerAmplitude = 1f;
    public float listenerFrequency = 1f;
    public float listenerDuration = 1f;
}

