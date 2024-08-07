using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour, IInteractSystem
{
    [SerializeField] private Rigidbody rb;
    private PlayerUI playerUI;
    private PlayerHeldItem playerHeldItem;
    private string text
    {
        get
        {
            if (!playerHeldItem.isHoldingItem)
            {
                return "Press F to Pickup Log";
            } 
            return string.Empty;
        }
    }

    public string promptText => text;

    void Start()
    {
        ObjectManagement.Instance.attachObject(gameObject);
        playerUI = FindObjectOfType<PlayerUI>();
        if (playerUI == null)
        {
            Debug.LogError("PlayerUI not found ");
        }
        playerHeldItem = FindObjectOfType<PlayerHeldItem>();
        if (playerHeldItem == null)
        {
            Debug.LogError("PlayerHeldItem not found ");
        }
    }

    public void Interact(InteractionSys player)
    {
        if (playerHeldItem.holdItem(gameObject))
        {
            Debug.Log("Item interacted and handled by PlayerHeldItem.");
        }
        else
        {
            Debug.Log("Failed to handle item.");
        }
    }
}