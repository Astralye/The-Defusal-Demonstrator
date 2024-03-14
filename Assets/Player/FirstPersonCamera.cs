using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class FirstPersonCamera : MonoBehaviour
{

    public Transform orientation;
    public static bool enableZRotation;
    public static bool leanRight;
    public static float pivotRotation;

    [SerializeField]
    private float minYrotation;

    [SerializeField]
    private float maxYrotation;

    private bool setPivotRotation;
    float xRotation;
    float yRotation;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        enableZRotation = false;
        setPivotRotation = false;
        leanRight = true;
    }

    // Update is called once per frame
    void Update()
    {

        float inputX = Input.GetAxis("Mouse X") * Time.deltaTime * GlobalSettings.mouseSensitivity;
        float inputY = Input.GetAxis("Mouse Y") * Time.deltaTime * GlobalSettings.mouseSensitivity;

        yRotation += inputX;

        xRotation -= inputY;
        xRotation = Mathf.Clamp(xRotation, -90.0f, 90.0f);

        if (enableZRotation)
        {
            rotatePivot();
        }
        else
        {
            setPivotRotation = true;

            transform.rotation = Quaternion.Euler(xRotation, yRotation, 0.0f);
            orientation.rotation = Quaternion.Euler(0, yRotation, 0);
        }
    }

    private void rotatePivot()
    {

        // rotation offset
        float rotationOffset = WallInteract.wallRotation;

        if (leanRight)
        {
            setRotationRight(rotationOffset);
        }
        else
        {
            setRotationLeft(rotationOffset);
        }

        if (setPivotRotation)
        {
            // Sets the start rotation
            yRotation = (leanRight) ? minYrotation : maxYrotation;
            setPivotRotation = false;
        }


        pivotRotation = Mathf.Clamp(yRotation, maxYrotation, minYrotation);

        if (yRotation < maxYrotation) yRotation = maxYrotation;
        else if (yRotation > minYrotation) yRotation = minYrotation;

        transform.rotation = Quaternion.Euler(xRotation, pivotRotation, transform.rotation.z);
    }
    public void setRotationLeft(float wallRotationOffset)
    {
        minYrotation = 70.0f;
        maxYrotation = -60.0f;

        maxYrotation += wallRotationOffset;
        minYrotation += wallRotationOffset;
    }

    public void setRotationRight(float wallRotationOffset)
    {
        minYrotation = 60.0f;
        maxYrotation = -70.0f;

        maxYrotation += wallRotationOffset;
        minYrotation += wallRotationOffset;
    }
}
