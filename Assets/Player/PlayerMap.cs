using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMap : MonoBehaviour
{
    private InputActions inputActions;

    private bool openedMap;

    // Start is called before the first frame update
    void Start()
    {
        inputActions = new InputActions();
        openedMap = false;
    }

    // Update is called once per frame
    void Update()
    {
        binds();
        //Debug.Log(openedMap);
    }


    void binds()
    {
        //inputActions.Player.Map.started += x => { openedMap = !openedMap; };
    }
}
