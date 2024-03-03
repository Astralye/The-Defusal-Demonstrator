using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteract : MonoBehaviour
{
    public Transform InteractorSource; // Where the raycast will be fired from

    [SerializeField]
    public float InteractRange;

    [SerializeField]
    private LayerMask mask;
    public PlayerUI playerUI;
    public Image hoverBackground;


    [Header("KeyBind")]
    public KeyCode interactButton = KeyCode.E;

    void Update()
    {
        playerUI.UpdateText(string.Empty);
        hoverBackground.enabled = false;
        Ray ray = new Ray(InteractorSource.position, InteractorSource.forward);
        RaycastHit hitInfo;

        // Check if raycast hits anything
        if (!Physics.Raycast(ray, out hitInfo, InteractRange, mask)) return;

        // Checks what object it is colliding with
        if (hitInfo.collider.GetComponent<Interactable>() == null) return;

        Interactable interactable = hitInfo.collider.GetComponent<Interactable>();
        if (!interactable.isEnabled) return;

        playerUI.UpdateText(interactable.hoverMessage);
        hoverBackground.enabled = true;
        if (Input.GetKeyDown(interactButton))
            interactable.BaseInteract();
    }
}
