using Repair;
using UnityEngine;

public class RepairMachineFocus : MonoBehaviour, IInteractable
{
    [SerializeField] private RepairSequenceController sequence;
    [SerializeField] private Transform focusPoint;
    [SerializeField] private Collider colliderToIgnore;
    [SerializeField] private RepairInteractionController repairController;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private string prompt = "repair";

    public string GetPrompt() => prompt;

    public void Highlight(bool state) { }

    public void Interact()
    {
        colliderToIgnore.enabled = false;
        repairController.EnterRepairMode(sequence, focusPoint, playerCamera);
    }
}
