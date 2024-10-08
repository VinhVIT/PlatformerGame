using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    public static PlayerInput playerInput;
    private Camera cam;

    public Vector2 RawMovementInput { get; private set; }
    public Vector2 RawDashDirectionInput { get; private set; }
    public Vector2Int DashDirectionInput { get; private set; }
    public int NormInputX { get; private set; }
    public int NormInputY { get; private set; }

    public bool JumpInput { get; private set; }
    public bool JumpInputStop { get; private set; }
    public bool DashInput { get; private set; }
    public bool DashInputStop { get; private set; }

    public bool GrabInput { get; private set; }
    public bool BlockInput { get; private set; }
    public bool RollInput { get; private set; }
    public bool AttackInput { get; private set; }
    public bool AttackInputStop { get; private set; }
    public bool RunInput { get; private set; }
    public bool HealInput { get; private set; }
    public bool HealInputStop { get; private set; }
    public bool BuffInput { get; private set; }
    public int BuffSlotInput { get; private set; }


    [SerializeField] private float inputHoldTime = 0.2f;

    private float jumpInputStartTime;
    private float dashInputStartTime;

    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        cam = Camera.main;
    }

    private void Update()
    {
        CheckJumpInputHoldTime();
        CheckDashInputHoldTime();
    }

    public void OnMoveInput(InputAction.CallbackContext context)
    {
        RawMovementInput = context.ReadValue<Vector2>();

        NormInputX = Mathf.RoundToInt(RawMovementInput.x);
        NormInputY = Mathf.RoundToInt(RawMovementInput.y);
    }

    public void OnJumpInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            JumpInput = true;
            JumpInputStop = false;
            jumpInputStartTime = Time.time;
        }

        if (context.canceled)
        {
            JumpInputStop = true;
        }
    }

    public void OnGrabInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            GrabInput = true;
        }

        if (context.canceled)
        {
            GrabInput = false;
        }
    }
    public void OnBlockInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            BlockInput = true;
        }
        else if (context.canceled)
        {
            BlockInput = false;
        }
    }
    public void OnRunInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            RunInput = true;
        }

        if (context.canceled)
        {
            RunInput = false;
        }
    }
    public void OnHealInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            HealInput = true;
            HealInputStop = false;
        }

        if (context.canceled)
        {
            HealInput = false;
            HealInputStop = true;
        }
    }
    public void OnDashInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            DashInput = true;
            DashInputStop = false;
            dashInputStartTime = Time.time;
        }
        else if (context.canceled)
        {
            DashInputStop = true;
        }
    }

    public void OnRollInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            RollInput = true;
        }
        else if (context.canceled)
        {
            RollInput = false;
        }
    }
    public void OnDashDirectionInput(InputAction.CallbackContext context)
    {
        RawDashDirectionInput = context.ReadValue<Vector2>();
        RawDashDirectionInput = cam.ScreenToWorldPoint((Vector3)RawDashDirectionInput) - transform.position;

        DashDirectionInput = Vector2Int.RoundToInt(RawDashDirectionInput.normalized);
    }
    public void OnAttackInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            AttackInput = true;
            AttackInputStop = false;
        }
        if (context.canceled)
        {
            AttackInput = false;
            AttackInputStop = true;
        }
    }
    public void OnBuffInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            BuffInput = true;
            int buffSlot;
            if (int.TryParse(context.control.name, out buffSlot))
            {
                BuffSlotInput = buffSlot;
            }
            StartCoroutine(ResetBuffInput());
        }
    }
    private IEnumerator ResetBuffInput()
    {
        yield return new WaitForSeconds(.2f);
            BuffInput = false;
    }
    public void UseJumpInput() => JumpInput = false;
    public void UseDashInput() => DashInput = false;

    private void CheckJumpInputHoldTime()
    {
        if (Time.time >= jumpInputStartTime + inputHoldTime)
        {
            JumpInput = false;
        }
    }

    private void CheckDashInputHoldTime()
    {
        if (Time.time >= dashInputStartTime + inputHoldTime)
        {
            DashInput = false;
        }
    }
    public static void DeactivatePlayerControl()
    {
        playerInput.currentActionMap.Disable();
    }
    public static void ActivatePlayerControl()
    {
        playerInput.currentActionMap.Enable();
    }
}