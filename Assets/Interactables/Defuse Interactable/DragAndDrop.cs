using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DragAndDrop : MonoBehaviour
{
    // This doesnt move in x + z axis.
    // Need to eventually remake this code 

    Vector3 mousePosition;
    [SerializeField]
    public Camera camera;
    private Rigidbody rb;

    private bool inspectItem;
    private Vector3 oldPosition;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        inspectItem = false;
        oldPosition = transform.position;
    }

    private Vector3 GetMousePos()
    {
        return camera.WorldToScreenPoint(transform.position);
    }

    private void OnMouseDown()
    {
        if (Input.GetKey(KeyCode.E))
        {
            inspectItem = true;
            oldPosition = transform.position;
            transform.position = camera.transform.position;
        }
        else
        {
            inspectItem = false;
            //transform.position = oldPosition;
        }


        if (inspectItem) { return; }
        Vector3 inversePos = new Vector3(
            Input.mousePosition.x,
            Input.mousePosition.y,
            Input.mousePosition.z);

        Debug.Log(Input.mousePosition.x +","+ Input.mousePosition.y + "," + Input.mousePosition.z);

        mousePosition = inversePos - GetMousePos();
        rb.useGravity = false;
    }

    private void OnMouseUp()
    {
        rb.useGravity = true;
    }

    private void OnMouseDrag()
    {
        if (inspectItem) { return; }
        Vector3 inversePos = new Vector3(
            Input.mousePosition.x,
            Input.mousePosition.y,
            Input.mousePosition.z);

        transform.position = camera.ScreenToWorldPoint(inversePos - mousePosition);
    }
}
