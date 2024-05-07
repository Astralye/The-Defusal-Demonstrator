using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Cinemachine;
using Sirenix.OdinInspector;
using StarterAssets;
using UnityEngine.InputSystem;
using Unity.VisualScripting;
using System;

public class Bomb : Interactable
{

    [Title("Cameras")]
    [SerializeField] private Camera camera;
    [SerializeField] private CinemachineVirtualCamera defuseCamera;
    [SerializeField] private CinemachineVirtualCamera PlayerCamera;
    [SerializeField] private CinemachineVirtualCamera AimCamera;

    [Title("Objective Text")]
    [SerializeField] public TextMeshProUGUI objectiveText;

    [Title("Masks")]
    [SerializeField] private LayerMask itemMask;
    [SerializeField] private LayerMask tableMask;

    [Title("KeyBinds")]
    [SerializeField] private StarterAssetsInputs playerController;
    [SerializeField] private PlayerInput playerInput;

    [SerializeField] public GameObject item;
    [SerializeField] private float holdHeight;

    [SerializeField] private float inspectDistanceX;
    [SerializeField] private float inspectDistanceY;
    [SerializeField] private float rotationSpeed;

    private GameObject heldItem;
    private bool itemHeld = false;
    private bool currentlyHolding = false;
    private bool inDefuse = false;

    private bool inspecting = false;

    protected override void Interact()
    {
        inDefuse = true;
        onEnter();

        // Do some initialization, like what items are spawned ...

        objectiveText.text = "X Disarm the bomb";
        objectiveText.fontStyle = FontStyles.Strikethrough;
    }

    private void Update()
    {
        if (!inDefuse) { return; }

        if (inspecting)
        {
            inspectItem();
        }
        else
        {
            dragAndDrop();
        }

    }

    private void inspectItem()
    {
        heldItem.transform.position = new Vector3(defuseCamera.transform.position.x - inspectDistanceX, defuseCamera.transform.position.y - inspectDistanceY, defuseCamera.transform.position.z);

        // For now using rotation, but try with input system.

        if (playerController.holding)
        {
            float xRotation = Input.GetAxis("Mouse X") * rotationSpeed;
            float yRotation = Input.GetAxis("Mouse Y") * rotationSpeed;

            heldItem.transform.Rotate(Vector3.down, xRotation, Space.World);
            heldItem.transform.Rotate(Vector3.right, yRotation);

            // Rotation is buggy. need to fix.
        }

        if (!playerController.inspect)
        {
            inspecting = false;
        }
    }

    private void dragAndDrop()
    {
        // Raycast Try change this to something for gamepads and mouses
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit raycastHit;

        // When player first clicks, look at layer mask of grabbable
        if (Physics.Raycast(ray, out raycastHit, 10f, itemMask.value) && !currentlyHolding && playerController.holding)
        {
            item.transform.position = raycastHit.point;
            holdItem(raycastHit);
        }
        // When player is holding, look at layer mask of default
        // Change position of item
        else if (Physics.Raycast(ray, out raycastHit, 10f, tableMask) && playerController.holding)
        {
            item.transform.position = raycastHit.point;
            heldItem.transform.position = new Vector3(raycastHit.point.x, raycastHit.point.y + holdHeight, raycastHit.point.z);

            // if player hits interact button, the item will go close to the camera.

            if (playerController.inspect)
            {
                inspecting = true;
                unfreezeRotation();
                return;
            }
        }
        else if (itemHeld && !playerController.holding)
        {
            dropItem();
        }

    }

    private void holdItem(RaycastHit raycast)
    {
        currentlyHolding = true;
        itemHeld = true;

        heldItem = raycast.transform.gameObject;

        heldItem.GetComponent<Rigidbody>().useGravity = false;
        freezeRotation();
    }

    private void dropItem()
    {
        currentlyHolding = false;
        itemHeld = false;

        heldItem.GetComponent<Rigidbody>().useGravity = true;
        unfreezeRotation();
    }

    private void freezeRotation()
    {
        heldItem.GetComponent<Rigidbody>().freezeRotation = true;
    }

    private void unfreezeRotation()
    {
        heldItem.GetComponent<Rigidbody>().freezeRotation = false;
    }

    public void onEnter()
    {
        // show cursor
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;

        // Disable player camera
        defuseCamera.enabled = true;
        PlayerCamera.enabled = false;
        AimCamera.enabled = false;

        // Disable player movement
        playerController.enabled = false;
        playerInput.SwitchCurrentActionMap("Defusal");
    }

    // If player defused, leaves or quits level..
    public void onExit()
    {
        // Remove cursor
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;

        // Enable player camera
        defuseCamera.enabled = false;
        PlayerCamera.enabled = true;
        AimCamera.enabled = true;

        // Enable player movement
        playerController.enabled = true;
        playerInput.SwitchCurrentActionMap("Player");
    }

}
