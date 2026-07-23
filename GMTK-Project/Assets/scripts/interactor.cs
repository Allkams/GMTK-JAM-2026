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
    [SerializeField] Transform InteractorSource;
    [SerializeField] float InteractRange;

    private IInteractable currentInteractable;

    void Update()
    {
        CheckForInteractables();
    }

    void CheckForInteractables()
    {
        IInteractable newInteractable = null;

        Ray r = new(InteractorSource.position, InteractorSource.forward);

        if(Physics.Raycast(r, out RaycastHit hitInfo, InteractRange))
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

    public void TryInteract(InputAction.CallbackContext ctx)
    {

        if(!ctx.performed)
        {
            return;
        }

        currentInteractable?.Interact();
    }
}
