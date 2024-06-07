using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// If you add this to an object with a collider the interaction system will work

// Notice the comma with the interface this will allow the player to see its interactable
// If you add the IInteractSystem to your script you wanna be able to interact with it won't let you run without Interact() func and the prompt Text.
public class KeyPickup : MonoBehaviour
{
    // This is the text that will be displayed as a prompt to the player
    [SerializeField] private string text = string.Empty;

    // This property sends the text to the player and is necessary.
    public string promptText => text;

    // Reference to the player's camera transform
    private Transform playerCameraTransform;

    // Function that will be called by the player and will run the code you write in here.
    public void Interact(InteractionSystem player)
    {
        Debug.Log("Interacting with the object.");

        // Get the player's camera transform
        playerCameraTransform = Camera.main.transform;

        // If the object is not already parented to the player, pick it up
        if (transform.parent != playerCameraTransform)
        {
            PickUp();
        }
        else // If the object is already parented to the player, place it down
        {
            PlaceDown();
        }
    }

    // Method to pick up the object and place it in front of the player's camera
    private void PickUp()
    {
        transform.SetParent(playerCameraTransform);
        transform.localPosition = new Vector3(0, 0, 2); // Adjust the position as needed
        transform.localRotation = Quaternion.identity;
        Debug.Log("Picked up the object.");
    }

    
    private void PlaceDown()
    {
        transform.SetParent(null);
        Debug.Log("Placed down the object.");
    }
}