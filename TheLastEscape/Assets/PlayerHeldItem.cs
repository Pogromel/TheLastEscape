using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHeldItem : MonoBehaviour
{
    #region Singleton
    public static PlayerHeldItem Instance;
    #endregion

    [SerializeField] private Transform _camera;
    public bool isHoldingItem = false;
    private GameObject heldItem;
    public GameObject HeldItem { get { return heldItem; }}

    private void Awake()
    {
        Instance = this;
    }

    public bool holdItem(GameObject item)
    {
        if (heldItem == null)
        {
            heldItem = item;
            isHoldingItem = true;
            item.layer = LayerMask.NameToLayer("HeldItems");
            item.transform.parent = _camera;
            return true;
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
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            if (heldItem != null)
            {
                DropItem();
            }
        }
    }
}