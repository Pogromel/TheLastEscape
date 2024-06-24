using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHitSystem
{
    void Hit(InteractionSys player, RaycastHit hit, GameObject heldItem);
}
public interface IInteractSystem
{
    string promptText { get; }
    void Interact(InteractionSys player);
}

public class InteractionSys : MonoBehaviour
{
    [SerializeField] private float _hitRange;
    [SerializeField] private float _interactionRange = 3f;
    [SerializeField] private float _distanceFromCamera = 1f;
    [SerializeField] private Transform _camera;
    [SerializeField] private PlayerUI _scriptPlayerUI;
    private bool _canSeePrompt;
    private PlayerHeldItem playerHeldItemScript;

    private void Start()
    {
        playerHeldItemScript = PlayerHeldItem.Instance;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (_canSeePrompt)
        {
            _canSeePrompt = _scriptPlayerUI.SetInteractionPrompt(string.Empty);
        }
        Ray r = new Ray(_camera.position + _camera.forward, _camera.forward);
        if (Physics.Raycast(r, out RaycastHit interactHit, _interactionRange))
        {
            if (interactHit.transform.TryGetComponent(out IInteractSystem interactSystem))
            {
                _canSeePrompt = _scriptPlayerUI.SetInteractionPrompt(interactSystem.promptText);
                if (Input.GetKeyDown(KeyCode.F))
                {
                    interactSystem.Interact(this);
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(_camera.position + (_camera.forward * _distanceFromCamera), _camera.position + (_camera.forward * _distanceFromCamera) + (_camera.forward * _hitRange));

        Gizmos.color = Color.green;
        Gizmos.DrawLine(_camera.position + (_camera.forward * _distanceFromCamera), _camera.position + (_camera.forward * _distanceFromCamera) + (_camera.forward * _interactionRange));
    }
}
