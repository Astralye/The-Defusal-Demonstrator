using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Bomb : Interactable
{
    [SerializeField]
    public PlayerData playerData;
    public TextMeshProUGUI objectiveText;

    Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    protected override void Interact()
    {
        if (!playerData.disarmed)
        {
            transform.position = gameObject.transform.position;
            rb.AddForce(transform.up * 5.0f, ForceMode.Impulse);

            playerData.disarmed = true;
            objectiveText.text = "X Disarm the bomb";
            objectiveText.fontStyle = FontStyles.Strikethrough;

            this.isEnabled = false;
        }
    }

}
