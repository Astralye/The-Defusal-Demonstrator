using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPerson : MonoBehaviour
{
    [SerializeField]
    public CinemachineVirtualCamera camera;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            camera.enabled = true;
        }
        else
        {
            camera.enabled = false;
        }
    }
}
