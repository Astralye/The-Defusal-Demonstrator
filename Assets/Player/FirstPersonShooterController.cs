using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using StarterAssets;
using UnityEngine.UI;

public class FirstPersonShooterController : MonoBehaviour
{
    [Header("Aiming")]
    [SerializeField] private CinemachineVirtualCamera aimVirtualCamera;
    [SerializeField] private float normalSensitivity;
    [SerializeField] private float aimSensitivity;

    [SerializeField] private LayerMask aimColliderMask = new LayerMask();
    [SerializeField] private Transform debugTransform;
    private Animator animator;

    [Header("Interaction")]
    // Interaction system
    [SerializeField] private float InteractRange;

    [SerializeField] private LayerMask interactMask;
    [SerializeField] private PlayerUI playerUI;
    [SerializeField] private Image hoverBackground;

    private FirstPersonController firstPersonController;
    private StarterAssetsInputs starterAssetsInputs;

    private void Awake()
    {
        firstPersonController = GetComponent<FirstPersonController>();
        starterAssetsInputs = GetComponent<StarterAssetsInputs>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        playerUI.UpdateText(string.Empty);
        hoverBackground.enabled = false;


        Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
        Transform hitTransform = null;

        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, aimColliderMask))
        {
            debugTransform.position = raycastHit.point;
            hitTransform = raycastHit.transform;
        }

        if (Physics.Raycast(ray, out RaycastHit hitInfo, InteractRange, interactMask))
        {
            interactable(hitInfo);
        }
        else
        {
            starterAssetsInputs.interact = false;
        }


        if (starterAssetsInputs.aim)
        {
            aimVirtualCamera.gameObject.SetActive(true);
            firstPersonController.SetSensitivity(aimSensitivity);
            //animator.SetBool("")
        }
        else
        {
            aimVirtualCamera.gameObject.SetActive(false);
            firstPersonController.SetSensitivity(normalSensitivity);
        }

        //if(starterAssetsInputs.attack)
        //{
        //    if (hitTransform != null)
        //    {

        //    }
        //}
    }

    private void interactable(RaycastHit hitInfo)
    {
        // Checks what object it is colliding with
        if (hitInfo.collider.GetComponent<Interactable>() == null) return;

        Interactable interactable = hitInfo.collider.GetComponent<Interactable>();
        if (!interactable.getEnabled() || !interactable.getToggle()) return;

        playerUI.UpdateText(interactable.hoverMessage);
        hoverBackground.enabled = true;

        if (starterAssetsInputs.interact) { 
            starterAssetsInputs.interact = false;
            interactable.BaseInteract();
        }
    }
}
