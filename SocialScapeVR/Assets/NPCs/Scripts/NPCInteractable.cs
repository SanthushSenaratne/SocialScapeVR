using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class NPCInteractable : MonoBehaviour
{
    [SerializeField] private String interactText;
    [SerializeField] private GameObject toActivate;
    [SerializeField] private GameObject toDeactivate;

    private Animator animator;
    private NPCHeadLookAt npcHeadLookAt;
    private FirstPersonController firstPersonController;
    private void Awake () {
        animator = GetComponent<Animator>();
        npcHeadLookAt = GetComponent<NPCHeadLookAt>();
        firstPersonController = FindObjectOfType<FirstPersonController>();
    }
    
    public void Interact(Transform interactorTransform) {

        firstPersonController.cameraCanMove = false;
        firstPersonController.playerCanMove = false;
        firstPersonController.enableJump = false;

        animator.SetTrigger("Talk");

        float playerHeight = 1f;
        npcHeadLookAt.LookAtPosition(interactorTransform.position + Vector3.up * playerHeight);
    
        toActivate.SetActive(true);
        toDeactivate.SetActive(false);

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public String GetInteractText() {
        return interactText;
    }

    public void StopInteract() {
        firstPersonController.cameraCanMove = true;
        firstPersonController.playerCanMove = true;
        firstPersonController.enableJump = true;

        animator.SetTrigger("Idle");

        npcHeadLookAt.ResetLookAt();
        
        toActivate.SetActive(false);
        toDeactivate.SetActive(true);
        
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
