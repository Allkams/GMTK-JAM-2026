using Repair;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RepairMachineFocus : MonoBehaviour, IInteractable
{
    [SerializeField] private RepairSequenceController sequence;
    [SerializeField] private Transform focusPoint;
    [SerializeField] private Collider colliderToIgnore;
    [SerializeField] private RepairInteractionController repairController;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private TextMeshProUGUI inputHelper;
    [SerializeField] private Image crosshair; 
    [SerializeField] private string prompt = "repair";

    public string GetPrompt() => prompt;

    public void Highlight(bool state) { }

    public void Interact()
    {
        colliderToIgnore.enabled = false;
        inputHelper.text = "Press ESC to leave";
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        repairController.EnterRepairMode(sequence, focusPoint, playerCamera);
        crosshair.enabled = false;
    }
}
