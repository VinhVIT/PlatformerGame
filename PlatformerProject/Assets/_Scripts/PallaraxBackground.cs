using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PallaraxBackground : MonoBehaviour
{   
    [SerializeField] private Vector2 parallaxEffectMultiplier;
    private Transform cameraTrans;
    private Vector3 lastCamPos;
    private void Start() {
        cameraTrans = Camera.main.transform;
        lastCamPos = cameraTrans.position;
    }
    void LateUpdate()
    {
        Vector3 deltaMovement = cameraTrans.position - lastCamPos;
        transform.position += new Vector3(deltaMovement.x * parallaxEffectMultiplier.x,deltaMovement.y * parallaxEffectMultiplier.y);
        lastCamPos = cameraTrans.position;
    }
}
