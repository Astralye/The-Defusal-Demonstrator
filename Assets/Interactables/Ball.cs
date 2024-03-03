using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : Interactable
{
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    protected override void Interact()
    {
        transform.position = gameObject.transform.position;
        rb.AddForce(transform.up * 5.0f, ForceMode.Impulse);
    }
}
