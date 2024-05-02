using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteract : MonoBehaviour
{
    private Transform InteractorSource; // Where the raycast will be fired from

    [SerializeField]
    private float InteractRange;

    [SerializeField] private LayerMask mask;
    [SerializeField] private PlayerUI playerUI;
    [SerializeField] private Image hoverBackground;

    private InputActions inputActions;
    private bool interactButton;

    private void Awake()
    {
        interactButton = false;

        inputActions = new InputActions();
        inputActions.Player.Enable();

        inputActions.Player.Interact.started += _ => { interactButton = true; };
        inputActions.Player.Interact.canceled += _ => { interactButton = false; };

        InteractorSource = GetComponent<Transform>();
    }

    void Update()
    {
        playerUI.UpdateText(string.Empty);
        hoverBackground.enabled = false;

        Ray ray = new Ray(InteractorSource.position, InteractorSource.forward);
        RaycastHit hitInfo;

        Debug.DrawRay(InteractorSource.position, InteractorSource.forward * 10);

        // Check if raycast hits anything
        if (Physics.Raycast(ray, out hitInfo, InteractRange, mask))
        {
            interactable(hitInfo);
        }
    }

    private void interactable(RaycastHit hitInfo)
    {

        // Checks what object it is colliding with
        if (hitInfo.collider.GetComponent<Interactable>() == null) return;

        Interactable interactable = hitInfo.collider.GetComponent<Interactable>();
        if (!interactable.getEnabled() || !interactable.getToggle()) return;

        playerUI.UpdateText(interactable.hoverMessage);
        hoverBackground.enabled = true;

        if (interactButton) { interactable.BaseInteract();}
    }
}
