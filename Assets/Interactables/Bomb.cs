using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Bomb : Interactable
{
    [SerializeField]
    public TextMeshProUGUI objectiveText;
    public DefuseMenu defuseMenu;

    public GameObject defaultCamera;
    public Camera defuseCamera;

    Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        defaultCamera.SetActive(true);
    }

    protected override void Interact()
    {
        if (!PlayerData.disarmed)
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;

            defaultCamera.SetActive(false);
            defuseCamera.enabled = true;

            transform.position = gameObject.transform.position;
            rb.AddForce(transform.up * 5.0f, ForceMode.Impulse);

            //playerData.disarmed = true;
            objectiveText.text = "X Disarm the bomb";
            objectiveText.fontStyle = FontStyles.Strikethrough;

            isEnabled = false;
        }
    }

}
