using System;
using System.Collections;
using System.Collections.Generic;
using OpenAI;
using UnityEngine;
using UnityEngine.InputSystem;

public class NPCInteractable : MonoBehaviour
{
    [SerializeField] private GameObject toActivate;
    [SerializeField] private GameObject toDeactivate;
    [SerializeField] private ChatGPT chatGPT;

    private Animator animator;
    private NPCHeadLookAt npcHeadLookAt;

    private void Awake () {
        animator = GetComponent<Animator>();
        npcHeadLookAt = GetComponent<NPCHeadLookAt>();
    }
    
    public void Interact(Transform interactorTransform) {

        float playerHeight = 0f;
        npcHeadLookAt.LookAtPosition(interactorTransform.position + Vector3.up * playerHeight);
    
        toActivate.SetActive(true);
        toDeactivate.SetActive(false);

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void StopInteract() {

        animator.SetTrigger("Idle");

        npcHeadLookAt.ResetLookAt();

        chatGPT.ClearMessages();
        
        toActivate.SetActive(false);
        toDeactivate.SetActive(true);
        
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
