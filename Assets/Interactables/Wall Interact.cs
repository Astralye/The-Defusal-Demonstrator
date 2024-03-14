using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class WallInteract : Interactable
{
    public enum leanType
    {
        leanLeft,
        leanRight
    }

    public leanType lean;
    public GameObject cameraHolder;
    public static float wallRotation;

    private Camera localCamera;

    [SerializeField]
    private float offsetX;
    [SerializeField]
    private float offsetZ;

    private GameObject pivot;
    private GameObject cameraPos;

    private bool cameraLock;
    private bool createPivot;

    void Start()
    {
        cameraLock = false;
        createPivot = true;
        wallRotation = 0.0f;

        localCamera = cameraHolder.GetComponentsInChildren<Camera>()[0];
    }

    private void Update()
    {
        if (cameraLock)
        {
            if (createPivot) createCameraHolder();
            rotateCamera();
        }

        if (cameraLock && Input.GetKey(KeyCode.F))
        {
            resetCamera();
        }
    }
    private void createCameraHolder()
    {

        pivot = new GameObject("Pivot");
        pivot.transform.rotation = transform.rotation;
        pivot.transform.position = transform.position;

        cameraPos = new GameObject("CameraPos");
        cameraPos.transform.SetParent(pivot.transform);


        wallRotation = transform.parent.transform.rotation.eulerAngles.y;
        float Angle = wallRotation * Mathf.Deg2Rad;

        // Rotating point with wall angle
        float newX = (offsetX * Mathf.Cos(-Angle)) - (offsetZ * Mathf.Sin(-Angle));
        float newZ = (offsetZ * Mathf.Cos(-Angle)) + (offsetX * Mathf.Sin(-Angle));

        Debug.Log("X part A: "+(offsetX * Mathf.Cos(Angle))+", X part B: "+ (offsetZ * Mathf.Sin(Angle)));
        if (lean == leanType.leanRight)
        {
            cameraPos.transform.position = new Vector3(
                pivot.transform.position.x + newX,
                pivot.transform.position.y,
                pivot.transform.position.z + newZ);

            FirstPersonCamera.leanRight = true;
        }
        else
        {

            Debug.Log(newZ);
            cameraPos.transform.position = new Vector3(
                pivot.transform.position.x + newX,
                pivot.transform.position.y,
                pivot.transform.position.z + newZ);

            Debug.Log("new position:" + newX + "," + newZ);

            FirstPersonCamera.leanRight = false;
        }

        createPivot = false;
    }

    private void rotateCamera()
    {
        FirstPersonCamera.enableZRotation = true;
        float zRotation;

        if (lean == leanType.leanRight)
        {
            zRotation = -10.0f;
        }
        else
        {
            zRotation = 10.0f;
        }

        localCamera.transform.Rotate(0.0f, 0.0f, zRotation);
        // Rotate the pivot around the wall.
        pivot.transform.rotation = Quaternion.Euler(0, FirstPersonCamera.pivotRotation, 0);
        cameraHolder.transform.position = cameraPos.transform.position;
    }

    private void resetCamera()
    {
        createPivot = true;
        cameraLock = false;
        FirstPersonCamera.enableZRotation = false;
        PlayerData.enablePlayerMovement = true;
        cameraHolder.GetComponent<MoveCamera>().enabled = true;

        Destroy(pivot);
        Destroy(cameraPos);

        toggleOn = true;
    }

    protected override void Interact()
    {
        createPivot = true;
        cameraLock = true;

        FirstPersonCamera.enableZRotation = true;
        PlayerData.enablePlayerMovement = false;
        cameraHolder.GetComponent<MoveCamera>().enabled = false;
        toggleOn = false;
    }
}
