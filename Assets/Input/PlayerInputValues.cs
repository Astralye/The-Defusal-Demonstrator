using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public class PlayerInputValues : MonoBehaviour
{

    // Public so other scripts can access action maps
    private InputActions playerMovement;

    [Header("Character Input Values")]
    public Vector2 move;
    public Vector2 look;
    public bool jump;
    public bool sprint;
    public bool aim;
    public bool attack;
    public bool crouch;

    public bool interact;

    public bool holdItem;
    public bool inspect;
    public bool altClick;

    [Header("Movement Settings")]
    public bool analogMovement;

    [Header("Mouse Cursor Settings")]
    public bool cursorLocked = true;
    public bool cursorInputForLook = true;

    private void Awake()
    {
        playerMovement = new InputActions();
    }

    public void OnEnable()
    {
        playerMovement.Enable();
        enablePlayerMovement();
    }

    public void OnDisable()
    {
        playerMovement.Disable();
        disablePlayerMovement();
        disableDefusal();
    }


    public void enablePlayerMovement()
    {
        playerMovement.Player.Enable();

        playerMovement.Player.Move.started += startMove;
        playerMovement.Player.Move.performed += startMove;
        playerMovement.Player.Move.canceled += stopMove;

        playerMovement.Player.Look.started += lookAround;
        playerMovement.Player.Look.performed += lookAround;
        playerMovement.Player.Look.canceled += stopLook;

        playerMovement.Player.Jump.started += startJump;

        playerMovement.Player.Sprint.started += startSprint;
        playerMovement.Player.Sprint.performed += startSprint;

        playerMovement.Player.Aim.started += startAim;
        playerMovement.Player.Aim.performed += startAim;

        playerMovement.Player.Interact.started += startInteract;

        playerMovement.Player.Attack.started += startAttack;
        playerMovement.Player.Attack.performed += startAttack;

        playerMovement.Player.Crouch.started += startCrouch;
        playerMovement.Player.Crouch.performed += startCrouch;
        playerMovement.Player.Crouch.canceled += startCrouch;
    }
    public void disablePlayerMovement()
    {
        playerMovement.Player.Disable();

        playerMovement.Player.Move.started -= startMove;
        playerMovement.Player.Move.performed -= startMove;
        playerMovement.Player.Move.canceled -= stopMove;

        playerMovement.Player.Look.started -= lookAround;
        playerMovement.Player.Look.performed -= lookAround;
        playerMovement.Player.Look.canceled -= stopLook;

        playerMovement.Player.Jump.started -= startJump;

        playerMovement.Player.Sprint.started -= startSprint;
        playerMovement.Player.Sprint.performed -= startSprint;

        playerMovement.Player.Aim.started -= startAim;
        playerMovement.Player.Aim.performed -= startAim;

        playerMovement.Player.Interact.started -= startInteract;

        playerMovement.Player.Attack.started -= startAttack;
        playerMovement.Player.Attack.performed -= startAttack;

        playerMovement.Player.Crouch.started += startCrouch;
        playerMovement.Player.Crouch.performed += startCrouch;
        playerMovement.Player.Crouch.canceled += startCrouch;
    }

    public void enableDefusal()
    {
        playerMovement.Defusal.Hold.started += startHold;
        playerMovement.Defusal.Hold.performed += startHold;
        playerMovement.Defusal.Hold.canceled += startHold;

        playerMovement.Defusal.Inspect.started += flipInspect;

        playerMovement.Defusal.Look.started += lookAround;
        playerMovement.Defusal.Look.performed += lookAround;
        playerMovement.Defusal.Look.canceled += stopLook;


        playerMovement.Defusal.AltClick.started += startAltClick;
        playerMovement.Defusal.AltClick.performed += startAltClick;
        playerMovement.Defusal.AltClick.canceled += startAltClick;
    }

    public void disableDefusal()
    {
        playerMovement.Defusal.Hold.started -= startHold;
        playerMovement.Defusal.Hold.performed -= startHold;
        playerMovement.Defusal.Hold.canceled -= startHold;

        playerMovement.Defusal.Inspect.started -= flipInspect;

        playerMovement.Defusal.Look.started -= lookAround;
        playerMovement.Defusal.Look.performed -= lookAround;
        playerMovement.Defusal.Look.canceled -= stopLook;

        playerMovement.Defusal.AltClick.started -= startAltClick;
        playerMovement.Defusal.AltClick.performed -= startAltClick;
        playerMovement.Defusal.AltClick.canceled -= startAltClick;

    }

    // Subscriber functions
    // --------------------------------------------------------------------------------------------

    // Player movement

    private void startMove(InputAction.CallbackContext value)
    {
        move = value.ReadValue<Vector2>().normalized;
    }

    private void stopMove(InputAction.CallbackContext value)
    {
        move = Vector2.zero;
    }

    private void lookAround(InputAction.CallbackContext value)
    {
        look = value.ReadValue<Vector2>();
    }

    private void stopLook(InputAction.CallbackContext value)
    {
        look = Vector2.zero;
    }

    private void startJump(InputAction.CallbackContext value)
    {
        if (value.ReadValue<float>() == 1.0f)
        {
            jump = true;
        }
    }

    private void startSprint(InputAction.CallbackContext value)
    {
        if (value.ReadValue<float>() == 1.0f)
        {
            sprint = true;
        }
        else
        {
            sprint = false;
        }
    }

    private void startAim(InputAction.CallbackContext value)
    {

        if (value.ReadValue<float>() == 1.0f)
        {
            aim = true;
        }
        else
        {
            aim = false;
        }
    }

    private void startInteract(InputAction.CallbackContext value)
    {
        interact = true;
    }

    private void startAttack(InputAction.CallbackContext value)
    {
        if (value.ReadValue<float>() == 1.0f)
        {
            attack = true;
        }
        else
        {
            attack = false;
        }
    }

    private void startCrouch(InputAction.CallbackContext value)
    {
        if (value.ReadValue<float>() == 1.0f)
        {
            crouch = true;
        }
        else
        {
            crouch = false;
        }
    }

    // Defusal Controls

    private void startHold(InputAction.CallbackContext value)
    {
        if (value.ReadValue<float>() == 1.0f)
        {
            holdItem = true;
        }
        else
        {
            holdItem = false;
        }
    }

    private void flipInspect(InputAction.CallbackContext value)
    {
        if (value.ReadValue<float>() == 1.0f)
        {
            inspect = !inspect;
        }
    }

    private void startAltClick(InputAction.CallbackContext value)
    {
        if (value.ReadValue<float>() == 1.0f)
        {
            altClick = true;
        }
        else
        {
            altClick = false;
        }
    }




    // ---------------------------------------------------------------------------------------------
    private void OnApplicationFocus(bool hasFocus)
    {
        SetCursorState(cursorLocked);
    }

    private void SetCursorState(bool newState)
    {
        Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
    }
}
