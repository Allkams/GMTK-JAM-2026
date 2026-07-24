using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

interface IInteractable
{
    public void Interact();
    void Highlight(bool state);

    string GetPrompt();
}

public class interactor : MonoBehaviour
{
    public enum RayOrigin
    {
        Forward,
        ScreenPoint
    }
    [SerializeField] Transform InteractorSource;
    [SerializeField] Camera RayCamera;
    [SerializeField] float InteractRange;

    [SerializeField] RayOrigin rayOrigin = RayOrigin.Forward;

    [SerializeField] InputActionReference interactAction;

    private IInteractable currentInteractable;
    private bool isLocked;

    void OnEnable() => interactAction.action.Enable();

    void OnDisable() => interactAction.action.Disable();

    void Update()
    {
        CheckForInteractables();
        TryInteract();
    }

    void CheckForInteractables()
    {
        if(isLocked) { return; }

        IInteractable newInteractable = null;

        if(Physics.Raycast(BuildRay(), out RaycastHit hitInfo, InteractRange))
        {
            hitInfo.collider.TryGetComponent(
                out newInteractable
            );
        }

        if(newInteractable != currentInteractable)
        {
            currentInteractable?.Highlight(false);

            currentInteractable = newInteractable;

            currentInteractable?.Highlight(true);
        }
    }

    Ray BuildRay()
    {
        if(rayOrigin == RayOrigin.ScreenPoint && RayCamera != null && Mouse.current != null)
        {
            return RayCamera.ScreenPointToRay(Mouse.current.position.ReadValue());    
        }

        return new(InteractorSource.position, InteractorSource.forward);
    }

    public void TryInteract()
    {
        if(!interactAction.action.WasPressedThisFrame())
        {
            return;
        }

        currentInteractable?.Interact();
    }

    public void SetLocked(bool locked)
    {
        isLocked = locked;
        if(locked)
        {
            currentInteractable?.Highlight(false);
            currentInteractable = null;
        }
    }
}
