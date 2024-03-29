using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    private float standingHeight;
    private float currentHeight;
    private float endHeight;

    [Header("Movement")]
    public float sprintSpeed;
    public float crouchSpeed;
    public float walkSpeed;
    public float crouchHeight;

    private float moveSpeed;
    public float groundDrag;

    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump;

    [Header("Stamina")]
    public float maxStamina;
    public float staminaCost;
    public float staminaRegen;

    public GameObject staminaBarContainer;
    private Image staminaBar;

    private float currentStamina;
    private float staminaBarWidth;
    private float currentStaminaBarWidth;

    [Header("Keybinds")]
    public KeyCode jumpKey   = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift;
    public KeyCode crouchKey = KeyCode.LeftControl;

    [Header("Ground Check")]
    public float playerHeight;
    public float setHeight;
    public LayerMask whatIsGround;
    bool grounded;

    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;

    public enum MovementType
    {
        walk,
        sprint,
        crouch,
        air
    }

    public MovementType movementType;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        staminaBar = staminaBarContainer.GetComponentsInChildren<Image>()[1];
        staminaBarWidth = staminaBar.rectTransform.rect.width;
        currentStamina = maxStamina;

        staminaBarContainer.SetActive(false);
        
        rb.freezeRotation = true;
        readyToJump = true;
        setHeight = playerHeight;

        movementType = MovementType.walk;
        moveSpeed = walkSpeed;

        standingHeight = 1.0f;
        currentHeight = standingHeight;
    }

    private void Update()
    {
        Vector3 dist = Vector3.down * setHeight * 0.5f;
        dist.y -= 0.2f;

        grounded = Physics.Raycast(orientation.position, dist, 1, whatIsGround);

        //if (PauseMenu.isPaused) return;
            
        MyInput();
        SpeedControl();

        if (grounded)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = 0;
        }

    }

    private void FixedUpdate()
    {
        MovePlayer();
    }
     
    private void Crouch()
    {
        moveSpeed = crouchSpeed;
        movementType = MovementType.crouch;
        endHeight = crouchHeight;

        if (currentHeight >= endHeight)
        {
            transform.localScale = new Vector3(transform.localScale.x, currentHeight, transform.localScale.z);
            currentHeight -= (Time.deltaTime * 1.5f) * endHeight;
        }
        else
        {
            currentHeight = crouchHeight;
        }
    }
    private void Uncrouch()
    {
        endHeight = standingHeight;

        if (currentHeight <= endHeight)
        {
            transform.localScale = new Vector3(transform.localScale.x, currentHeight, transform.localScale.z);
            currentHeight += (Time.deltaTime * 1.5f) * endHeight;
        }
        else
        {
            currentHeight = standingHeight;
        }
    }

    private void RegenStamina()
    {
        // Stamina is the max
        if(currentStamina >= maxStamina)
        {
            currentStamina = maxStamina;
            staminaBarContainer.SetActive(false);
        }

        // Stamina is less than the max
        else
        {
            currentStamina += Time.deltaTime * staminaRegen;
            staminaBarContainer.SetActive(true);
            displayStamina();
        }
    }
    private void Sprint()
    {
        if(currentStamina <= 0)
        {
            movementType = MovementType.walk;
            moveSpeed = walkSpeed;
            currentStamina = 0;
        }
        else
        {
            movementType = MovementType.sprint;
            moveSpeed = sprintSpeed;
            currentStamina -= Time.deltaTime * staminaCost;
        }

        staminaBarContainer.SetActive(true);
        displayStamina();
    }

    private void displayStamina()
    {
        RectTransform rt = staminaBar.rectTransform;
        float percent = currentStamina / maxStamina;

        float width = Mathf.Lerp(0, staminaBarWidth, percent);
        // Change StaminaBar size
        rt.sizeDelta = new Vector3(width, rt.rect.height);
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKey(jumpKey) && readyToJump && grounded)
        {
            readyToJump = false;
            movementType = MovementType.air;
            Jump();

            // When the timer has run out, run the function
            Invoke(nameof(ResetJump), jumpCooldown);
        }

        if (Input.GetKey(sprintKey) && grounded && (movementType != MovementType.crouch))
        {
            Sprint();
        }
        else
        {
            RegenStamina();
        }

        //Action Delegate, Like a ternary operator
        (Input.GetKey(crouchKey) ? (Action)Crouch : Uncrouch)();


        // Only runs if no inputs
        if (!(Input.GetKey(sprintKey) || Input.GetKey(crouchKey)))
        {
            movementType = MovementType.walk;
            moveSpeed = walkSpeed;
        }

    }

    private void MovePlayer()
    {
        // Calculate movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // On the ground
        if (grounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10.0f, ForceMode.Force);
        }
        // In the air
        else if (!grounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * airMultiplier, ForceMode.Force);
        }
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
