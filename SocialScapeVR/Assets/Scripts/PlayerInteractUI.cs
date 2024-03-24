using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class PlayerInteractUI : MonoBehaviour
{
    [SerializeField] private GameObject containerGameObject;
    [SerializeField] private PlayerInteract playerInteract;
    [SerializeField] private TextMeshProUGUI interactTextMeshProGUI;
    [SerializeField] private String interactText;

    private void Update() {
        if (playerInteract.GetInteractableObject() != null) {
            Show(playerInteract.GetInteractableObject());
        } else {
            Hide();
        }
    }

    private void Show(NPCInteractable npcInteractable) {
        containerGameObject.SetActive(true);
        interactTextMeshProGUI.text = interactText;
    }

    private void Hide() {
        containerGameObject.SetActive(false);
    }
}
