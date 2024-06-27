using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySystem : MonoBehaviour
{
    public static InventorySystem Instance;

    [SerializeField] private List<Image> inventorySlots;
    private int currentSlotIndex = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public bool AddItem(Sprite ItemSprite)
    {
        if (currentSlotIndex < inventorySlots.Count)
        {
            inventorySlots[currentSlotIndex].sprite = ItemSprite;
            inventorySlots[currentSlotIndex].color = Color.white;
            currentSlotIndex++;
            Debug.Log("Item added to inventory.");
            return true;
        }
        Debug.Log("Inventory is full.");

        return false;
    }
    
}
