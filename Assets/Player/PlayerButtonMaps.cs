using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using static UnityEditor.PlayerSettings;
using static UnityEngine.UI.Image;

public class PlayerButtonMaps : MonoBehaviour
{
    public GameObject prefab; 

    private Transform playerPos;
    // Update is called once per frame
    private GameObject itemOut;
    private GameObject pivot;

    private bool ADS;

    private Vector3 defaultPosition;
    private Vector3 aimPosition;
    private Vector3 origin;

    private void Start()
    {
        itemOut = GetComponent<GameObject>();
        playerPos = GetComponent<Transform>();

        defaultPosition = new Vector3(-0.35f, 0.05f, -0.6f);
        aimPosition = new Vector3(1.0f, 0.18f, -0.5f);
    }

    void Update()
    {
        if (PlayerData.getItem)
        {
            displayItem();
        }
        //if (itemOut != null)
        //{
        //    origin = pivot.transform.position;

        //    GameObject orientation = GameObject.Find("PlayerCamera");
        //    Vector3 angle = orientation.transform.rotation.eulerAngles;
        //    angle.y -= 180;
        //    angle.x = -angle.x;
        //    pivot.transform.rotation = Quaternion.Euler(angle);

        //    if (ADS)
        //    {
        //        orientation.GetComponent<Camera>().fieldOfView = 40;
        //    }
        //    else
        //    {
        //        orientation.GetComponent<Camera>().fieldOfView = 60;
        //    }
        //}

    }
    private void displayItem()
    {
        PlayerData.getItem = false; // This isnt good for implementing the inventory, remove later

        //itemOut = Instantiate(prefab, playerPos.position, Quaternion.identity);

        //GameObject parent = GameObject.Find("PlayerCamera");


        //pivot = new GameObject("Pivot");
        //pivot.transform.rotation = transform.rotation;
        //pivot.transform.position = transform.position;
        //origin = pivot.transform.position;

        //pivot.transform.SetParent(parent.transform);
        //itemOut.transform.SetParent(pivot.transform);

        //itemPOV(defaultPosition);
    }

    private void itemPOV(Vector3 vec3)
    {
        itemOut.transform.position = new Vector3(
            origin.x + vec3.x,
            origin.y + vec3.y,
            origin.z + vec3.z);
    }
}
