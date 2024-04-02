using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    NPCInteractable currentInteractable;
    private void Update() 
    {
        if (Input.GetButtonDown("Interact")) 
        {
            currentInteractable = GetInteractableObject();
            if (currentInteractable != null)
            {
                currentInteractable.Interact(transform);
            }
        }
        else if (Input.GetButtonDown("Back"))
        {
            if (currentInteractable != null)
            {
                currentInteractable.StopInteract();
                currentInteractable = null;
            }
        }
        if (GetInteractableObject() != null) 
        {
            if (currentInteractable != GetInteractableObject())
            {
                if (currentInteractable != null)
                {
                    currentInteractable.StopInteract();
                    currentInteractable = null;
                }
            }
        }
    }

    public NPCInteractable GetInteractableObject() 
    {
        List<NPCInteractable> npcInteractableList = new List<NPCInteractable>();
        float InteractRange = 2f;
        Collider[] colliderArray = Physics.OverlapSphere(transform.position, InteractRange);
        foreach (Collider collider in colliderArray) 
        {
            if (collider.TryGetComponent(out NPCInteractable npcInteractable)) 
            {
                npcInteractableList.Add(npcInteractable);
            }
        }
        NPCInteractable closestInteractable = null;
        foreach (NPCInteractable npcInteractable in npcInteractableList) 
        {
           if (closestInteractable == null)
            {
                closestInteractable = npcInteractable;
                continue;
            }

            float squaredDistanceToInteractable = (transform.position - npcInteractable.transform.position).sqrMagnitude;
            float squaredDistanceToClosestInteractable = (transform.position - closestInteractable.transform.position).sqrMagnitude;

            if (squaredDistanceToInteractable < squaredDistanceToClosestInteractable)
            {
                closestInteractable = npcInteractable;
            }
        }
        return closestInteractable;
    }

    public void StopInteract() 
    {
        if (currentInteractable != null)
            {
                currentInteractable.StopInteract();
                currentInteractable = null;
            }
    }
}
