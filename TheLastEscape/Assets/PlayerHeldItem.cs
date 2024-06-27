using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHeldItem : MonoBehaviour
{
    #region Singleton
    public static PlayerHeldItem Instance;
    #endregion

    [SerializeField] private Transform _camera;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float pushForce = 10f;

    public bool isHoldingItem = false;
    private GameObject heldItem;
    public GameObject HeldItem { get { return heldItem; }}
    private bool isMovingItem = false;

    private void Awake()
    {
        Instance = this;
    }

    public bool holdItem(GameObject item)
    {
        if (heldItem == null)
        {
            if (item.CompareTag("key"))
            {
                InventorySystem inventory = InventorySystem.Instance;
                SpriteRenderer renderer = item.GetComponent<SpriteRenderer>();
                if (renderer != null && inventory.AddItem(renderer.sprite))
                {
                    Debug.Log("Key item added to inventory.");
                    Destroy(item);
                    return true;
                }
                else
                {
                    Debug.Log("Failed to add key item to inventory.");
                }
            }
            else if (item.CompareTag("object"))
            {
                heldItem = item;
                isHoldingItem = true;
                item.layer = LayerMask.NameToLayer("HeldItems");
                item.transform.parent = _camera;
                Debug.Log("Object item held.");
                return true;
            }
        }
        return false;
    }

    private void DropItem()
    {
        if (heldItem != null)
        {
            heldItem.transform.parent = null;
            if (heldItem.TryGetComponent(out Rigidbody rb))
            {
                rb.isKinematic = false;
                rb.useGravity = true;
            }
            heldItem.layer = LayerMask.NameToLayer("Default");
            ObjectManagement.Instance.attachObject(heldItem);
            heldItem = null;
            isHoldingItem = false;
            isMovingItem = false;
            Debug.Log("Object item dropped.");
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && isHoldingItem)
        {
            isMovingItem = !isMovingItem;
            Debug.Log("Toggled item movement: " + isMovingItem);
        }

        if (isMovingItem && heldItem != null)
        {
            float moveHorizontal = Input.GetAxis("Mouse X") * moveSpeed * Time.deltaTime;
            float moveVertical = Input.GetAxis("Mouse Y") * moveSpeed * Time.deltaTime;
            heldItem.transform.Translate(new Vector3(moveHorizontal, moveVertical, 0), _camera);
        }

        if (Input.GetMouseButtonDown(1) && heldItem != null)
        {
            Rigidbody heldRb = heldItem.GetComponent<Rigidbody>();
            if (heldRb != null)
            {
                heldItem.transform.parent = null;
                heldRb.isKinematic = false;
                heldRb.useGravity = true;
                heldRb.AddForce(_camera.forward * pushForce, ForceMode.Impulse);
                heldItem.layer = LayerMask.NameToLayer("Default");
                ObjectManagement.Instance.attachObject(heldItem);
                Debug.Log("Object item pushed away.");
                heldItem = null;
                isHoldingItem = false;
                isMovingItem = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            if (heldItem != null)
            {
                DropItem();
            }
        }
    }
}
