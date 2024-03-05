using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInteractable : MonoBehaviour
{
    [SerializeField] private String interactText;

    private Animator animator;
    private NPCHeadLookAt npcHeadLookAt;
    private void Awake () {
        animator = GetComponent<Animator>();
        npcHeadLookAt = GetComponent<NPCHeadLookAt>();
    }
    
    public void Interact(Transform interactorTransform) {
        animator.SetTrigger("Talk");

        float playerHeight = 1f;
        npcHeadLookAt.LookAtPosition(interactorTransform.position + Vector3.up * playerHeight);
    }

    public String GetInteractText() {
        return interactText;
    }
}
