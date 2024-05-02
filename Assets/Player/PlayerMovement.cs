using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Drawing;
using UnityEditor;
using UnityEngine.InputSystem.iOS;

public class PlayerMovement : MonoBehaviour
{
    private InputActions inputActions;
    [SerializeField] private LayerMask aimColliderMask = new LayerMask();
    [SerializeField] private Transform debugTransform;
    // Refactor this
    // hard to debug + understand

    [SerializeField] private float standingHeight;
    //private float currentHeight;
    //private float endHeight;

    //[Header("Movement")]
    //public float sprintSpeed;
    //public float crouchSpeed;
    //public float crouchHeight;

    // Movement speeds
    [SerializeField] private float moveSpeed; // current speed
    [SerializeField] private float walkSpeed; // static
    [SerializeField] public float groundDrag; // Multiplier

    // Jumping
    [SerializeField] private float jumpForce;
    [SerializeField] private float jumpCooldown;
    [SerializeField] private float airMultiplier;
    private bool readyToJump;

    // Stamina values
    [SerializeField] private float maxStamina;
    [SerializeField] private float staminaCost;
    [SerializeField] private float staminaRegen;
    private float currentStamina;

    // Stamina bar
    [SerializeField] private GameObject staminaBarContainer;
    private Image staminaBar;

    private float staminaBarWidth;
    //private float currentStaminaBarWidth;

    //[Header("Keybinds")]
    //public KeyCode jumpKey   = KeyCode.Space;

    // Movement flags
    public bool isWalking;
    public bool isSprint;
    public bool isCrouch;
    public bool isJump;
    public bool onGround;

    private bool isAiming = false;

    //[Header("Ground Check")]
    private float playerHeight;
    private float setHeight;
    [SerializeField] public LayerMask whatIsGround;

    public Transform orientation;
    [SerializeField] private GameObject playerModel;
    [SerializeField] private GameObject cameraOrientation;

    Vector3 moveDirection;

    Rigidbody rb;

    private Animator animator;

    public enum MovementType
    {
        walk,
        sprint,
        crouch,
        air
    }

    public MovementType movementType;

    // Initializers & Loop functions
    // ----------------------------------------------------------------------------------------------
    private void Awake()
    {
        inputActions = new InputActions();
        inputActions.Player.Enable();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        currentStamina = maxStamina;

        staminaBarContainer.SetActive(false);
        staminaBar = staminaBarContainer.transform.GetChild(0).GetComponent<Image>();

        //staminaBarWidth = staminaBar.rectTransform.rect.width;

        playerHeight = GetComponent<CapsuleCollider>().height;

        setHeight = GetComponent<CapsuleCollider>().height;

        movementType = MovementType.walk;
        moveSpeed = walkSpeed;

        standingHeight = 1.0f;
        //currentHeight = standingHeight;

        // Flags
        readyToJump = true;

        animator = GameObject.Find("PlayerModel").GetComponent<Animator>();


        isWalking = false;
        isSprint = false;
        isCrouch = false;
        isJump = false;
    }

    private void playerMovementActions()
    {
        inputActions.Player.Sprint.performed += x => { isSprint = true; };
        inputActions.Player.Sprint.canceled += x => { isSprint = false; };

        inputActions.Player.Crouch.performed += _ => { isCrouch = true; };
        inputActions.Player.Crouch.canceled += _ => { isCrouch = false; };

        inputActions.Player.Jump.started += _ => { isJump = true; };
        inputActions.Player.Jump.canceled += _ => { isJump = false; };

        inputActions.Player.ADS.started += _ => { isAiming = true; };
        inputActions.Player.ADS.performed += _ => { isAiming = true; };
        inputActions.Player.ADS.canceled += _ => { isAiming = false; };
    }

    private void Update()
    {
        if (PauseMenu.isPaused) { return; }
        playerMovementActions();

        if (!PlayerData.enablePlayerMovement) return;

        checkOnGround();
        MyInput();
    }

    // Physics based functions should be here.
    private void FixedUpdate()
    {
        MovePlayer();
    }
    
    // Checks
    // ----------------------------------------------------------------------------------------------------------

    private void checkOnGround()
    {
        Vector3 dist = Vector3.down * setHeight * 0.5f;

        if(Physics.Raycast(orientation.position, dist, 1.2f, whatIsGround))
        {
            rb.drag = groundDrag;
            onGround = true;
        }
        else
        {
            rb.drag = 0;
            onGround = false;
        }
    }

    //private void Crouch()
    //{


    //    Debug.Log(moveSpeed);
    //    moveSpeed = crouchSpeed;
    //    Debug.Log(moveSpeed);
    //    movementType = MovementType.crouch;
    //    endHeight = crouchHeight;

    //    if (currentHeight >= endHeight)
    //    {
    //        transform.localScale = new Vector3(transform.localScale.x, currentHeight, transform.localScale.z);
    //        currentHeight -= (Time.deltaTime * 1.5f) * endHeight;
    //    }
    //    else
    //    {
    //        currentHeight = crouchHeight;
    //    }
    //}
    //private void Uncrouch()
    //{
    //    endHeight = standingHeight;

    //    if (currentHeight <= endHeight)
    //    {
    //        transform.localScale = new Vector3(transform.localScale.x, currentHeight, transform.localScale.z);
    //        currentHeight += (Time.deltaTime * 1.5f) * endHeight;
    //    }
    //    else
    //    {
    //        currentHeight = standingHeight;
    //    }
    //}

    //private void RegenStamina()
    //{
    //    // Stamina is the max
    //    if(currentStamina >= maxStamina)
    //    {
    //        currentStamina = maxStamina;
    //        staminaBarContainer.SetActive(false);
    //    }

    //    // Stamina is less than the max
    //    else
    //    {
    //        currentStamina += Time.deltaTime * staminaRegen;
    //        staminaBarContainer.SetActive(true);
    //        displayStamina();
    //    }
    //}
    //private void Sprint()
    //{
    //    if(currentStamina <= 0)
    //    {
    //        movementType = MovementType.walk;
    //        moveSpeed = walkSpeed;
    //        currentStamina = 0;
    //    }
    //    else
    //    {
    //        movementType = MovementType.sprint;
    //        moveSpeed = sprintSpeed;
    //        currentStamina -= Time.deltaTime * staminaCost;
    //    }

    //    staminaBarContainer.SetActive(true);
    //    displayStamina();
    //}

    //private void displayStamina()
    //{
    //    RectTransform rt = staminaBar.rectTransform;
    //    float percent = currentStamina / maxStamina;

    //    float width = Mathf.Lerp(0, staminaBarWidth, percent);
    //    // Change StaminaBar size
    //    rt.sizeDelta = new Vector3(width, rt.rect.height);
    //}

    private void MyInput()
    {

        if (isJump && readyToJump && onGround)
        {
            readyToJump = false;
            movementType = MovementType.air;
            Jump();

            // When the timer has run out, run the function
            Invoke(nameof(ResetJump), jumpCooldown);
        }

        //Action Delegate, Like a ternary operator
        //(isSprint && grounded && (movementType != MovementType.crouch) ? (Action)Sprint : RegenStamina)();
        //(isCrouch ? (Action)Crouch : Uncrouch)();


        //// Only runs if no inputs
        //if (!isSprint && !isCrouch)
        //{
        //    movementType = MovementType.walk;
        //    moveSpeed = walkSpeed;
        //}
        aim();
    }
    private void aim()
    {


        Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);

        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);


        if(Physics.Raycast(ray, out RaycastHit raycastHit, 999f, aimColliderMask))
        {
            debugTransform.position = raycastHit.point;
        }



        Debug.Log(isAiming);


        // Change animation to aiming if true
        // change the animation back to idle if false
    }

    private void MovePlayer()
    {
        Vector2 inputVector = inputActions.Player.Walk.ReadValue<Vector2>();

        // Calculate movement direction
        moveDirection = orientation.forward * inputVector.y + orientation.right * inputVector.x;

        // Set player model rotation
        playerModel.transform.eulerAngles = orientation.eulerAngles;
        cameraOrientation.transform.eulerAngles = orientation.eulerAngles;

        // On the ground
        if (onGround)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10.0f, ForceMode.Force);
        }
        // In the air
        else if (!onGround)
        {
            //if (rb.velocity.normalized.sqrMagnitude < moveSpeed) { return; }

            rb.AddForce(moveDirection.normalized * moveSpeed * airMultiplier, ForceMode.Force);
        }

        updateAnimation(inputVector);

        SpeedControl();
    }

    private void updateAnimation(Vector2 inputVector)
    {

        animator.SetInteger("StrafeValue", (int)inputVector.x);
        animator.SetInteger("WalkValue", (int)inputVector.y);
    }


    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0.0f, rb.velocity.z);

        if(flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0.0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }
    private void ResetJump()
    {
        readyToJump = true;
    }

}
