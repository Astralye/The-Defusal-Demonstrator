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
    [SerializeField] private LayerMask bombInteractionMask;
    [SerializeField] private LayerMask WireMask;

    [Title("KeyBinds")]
    [SerializeField] private PlayerInputValues playerInputValues;
    [SerializeField] private PlayerInput playerInput;

    [SerializeField] public GameObject item;
    [SerializeField] private float holdHeight;

    [SerializeField] private float inspectDistanceX;
    [SerializeField] private float inspectDistanceY;

    [SerializeField] private float offhandDistanceX;
    [SerializeField] private float offhandDistanceY;
    [SerializeField] private float offhandDistanceZ;

    [SerializeField] private float rotationSpeedMultiplier;

    private GameObject heldItem;
    private GameObject offhandItem;
    private GameObject tmp;

    private bool itemHeld = false;
    private bool currentlyHolding = false;
    private bool inDefuse = false;

    private bool offhandItemHold = false;
    private bool inspecting = false;

    private bool validWirePos = false;

    private Wire wire;


    private void Start()
    {
        tmp = new GameObject("tmp");
        tmp.AddComponent<Rigidbody>();

        offhandItem = tmp;
    }

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


        if (offhandItemHold)
        {
            offhand();
        }

        if (inspecting)
        {
            inspectItem();
            altArmInteract();
        }
        else
        {
            dragAndDrop();
        }

    }

    private void offhand()
    {
        // If not holding interact button
        if (!playerInputValues.rightHandInteracting || wire == null)
        {
            offhandItem.transform.SetParent(GameObject.Find("Defusal Items").transform);

            offhandItem.transform.position = new Vector3(
                defuseCamera.transform.position.x - offhandDistanceX,
                defuseCamera.transform.position.y - offhandDistanceY, 
                defuseCamera.transform.position.z - offhandDistanceZ);

            offhandItem.GetComponent<Rigidbody>().freezeRotation = true;
        }
        else
        {
            offhandItem.transform.position = new Vector3(
                defuseCamera.transform.position.x - offhandDistanceX,
                defuseCamera.transform.position.y - offhandDistanceY,
                defuseCamera.transform.position.z - offhandDistanceZ);


            offhandItem.transform.LookAt(wire.transform);
            offhandItem.GetComponent<Rigidbody>().freezeRotation = true;
        }

        // if the player has no offhand item
        if (!playerInputValues.offhand)
        {
            //offhandItemHold = false;

            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit raycastHit;

            // When player first clicks, look at layer mask of grabbable
            if (Physics.Raycast(ray, out raycastHit, 10f, tableMask.value))
            {
                offhandItem.transform.position = raycastHit.point;
            }

            offhandItemHold = false;
            offhandItem.GetComponent<Rigidbody>().freezeRotation = false;
            offhandItem = tmp;

        }
    }

    private void inspectItem()
    {

        heldItem.transform.position = new Vector3(defuseCamera.transform.position.x - inspectDistanceX, defuseCamera.transform.position.y - inspectDistanceY, defuseCamera.transform.position.z);

        if (playerInputValues.rightHandInteracting) {
            freezeRotation();

            return;
        }
        else
        {
            unfreezeRotation();
        }

        if (playerInputValues.holdItem)
        {
            if (playerInputValues.altClick)
            {
                // Rotate around local z axis
                heldItem.transform.Rotate(-playerInputValues.look.y * rotationSpeedMultiplier, 0, -playerInputValues.look.x * rotationSpeedMultiplier, Space.Self);
            }
            else
            {
                // rotate around global y and z axis.
                heldItem.transform.Rotate(0, -playerInputValues.look.x * rotationSpeedMultiplier, -playerInputValues.look.y * rotationSpeedMultiplier, Space.World);
            }

            if (playerInputValues.offhand)
            {
                if (offhandItemHold) { return; }

                playerInputValues.inspect = false;
                playerInputValues.holdItem = false;

                offhandItemHold = true;
                offhandItem = heldItem;
                heldItem = tmp;
                return;
            }
        }

        if (!playerInputValues.inspect)
        {
            inspecting = false;
        }
    }

    private void altArmInteract()
    {
        // Let cursor do the raycast
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit raycastHit, raycastHit2;


        // No item hold
        if (Physics.Raycast(ray, out raycastHit, 10f, bombInteractionMask.value) && !offhandItemHold)
        {
            if (playerInputValues.playerClicking)
            {
                playerInputValues.playerClicking = false;
                raycastHit.collider.GetComponent<KeypadCode>().BaseInteract();
            }
        }

        // Function only runs when player is holding right hand interact.
        if (!(playerInputValues.rightHandInteracting && offhandItemHold)) { return; }

        if (Physics.Raycast(ray, out raycastHit2, 10f, WireMask.value))
        {
            item.transform.position = raycastHit2.point;

            wire = raycastHit2.collider.GetComponent<Wire>();

            if (playerInputValues.playerClicking)
            {
                Debug.Log("click");
                playerInputValues.playerClicking = false;
                wire.BaseInteract();
            }
        }
    }

    private void dragAndDrop()
    {
        playerInputValues.playerClicking = false;

        // Raycast Try change this to something for gamepads and mouses
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit raycastHit;

        // When player first clicks, look at layer mask of grabbable
        if (Physics.Raycast(ray, out raycastHit, 10f, itemMask.value) && !currentlyHolding && playerInputValues.holdItem)
        {
            item.transform.position = raycastHit.point;
            holdItem(raycastHit);
        }
        // When player is holding, look at layer mask of default
        // Change position of item
        else if (Physics.Raycast(ray, out raycastHit, 10f, tableMask) && currentlyHolding && playerInputValues.holdItem)
        {
            item.transform.position = raycastHit.point;
            heldItem.transform.position = new Vector3(raycastHit.point.x, raycastHit.point.y + holdHeight, raycastHit.point.z);

            // if player hits interact button, the item will go close to the camera.

            if (playerInputValues.inspect)
            {
                inspecting = true;
                unfreezeRotation();
                return;
            }

            if (playerInputValues.offhand)
            {
                if(offhandItemHold) { return; }

                offhandItemHold = true; 
                offhandItem = heldItem;
                return;
            }
        }
        else if (itemHeld && !playerInputValues.holdItem)
        {
            dropItem();
        }

    }

    private void holdItem(RaycastHit raycast)
    {
        if(offhandItem == raycast.transform.gameObject) { return; }

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
        heldItem.GetComponent<Rigidbody>().velocity = Vector3.zero;

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
        playerInputValues.inspect = false;

        // show cursor
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;

        // Disable player camera
        defuseCamera.enabled = true;
        PlayerCamera.enabled = false;
        AimCamera.enabled = false;

        // Disable player movement
        playerInputValues.disablePlayerMovement();
        playerInputValues.enableDefusal();

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
        playerInputValues.enablePlayerMovement();
        playerInputValues.disableDefusal();

        playerInput.SwitchCurrentActionMap("Player");
    }

}
