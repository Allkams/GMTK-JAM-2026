using UnityEngine;
using UnityEngine.InputSystem;

namespace Repair
{
    public class RepairInteractionController : MonoBehaviour
    {
        [SerializeField] Camera repairCamera;
        [SerializeField] InputActionReference exitZoom;

        private RepairSequenceController activeSequence;
        private Camera playerCamera;
        private bool isZoomed;

        private void OnEnable() => exitZoom.action.Enable();

        private void OnDisable() => exitZoom.action.Disable();

        public void EnterRepairMode(
            RepairSequenceController sequence,
            Transform focusPoint,
            Camera worldCamera
        )
        {
            activeSequence = sequence;
            playerCamera = worldCamera;

            playerCamera.gameObject.SetActive(false);
            repairCamera.gameObject.SetActive(true);
            repairCamera.transform.SetPositionAndRotation(focusPoint.position, focusPoint.rotation);

            activeSequence.OnSequenceCompleted.AddListener(LeaveRepairMode);

            isZoomed = true;
        }

        private void Update()
        {
            if(isZoomed && exitZoom.action.WasPressedThisFrame())
            {
                LeaveRepairMode();
            }
        }

        private void LeaveRepairMode()
        {
            if(!isZoomed) {return;}

            isZoomed = false;
            activeSequence.OnSequenceCompleted.RemoveListener(LeaveRepairMode);
            activeSequence.NotifyLeavingRepairing();
            repairCamera.gameObject.SetActive(false);
            playerCamera.gameObject.SetActive(true);
            activeSequence = null;
        }
    }
    
}
