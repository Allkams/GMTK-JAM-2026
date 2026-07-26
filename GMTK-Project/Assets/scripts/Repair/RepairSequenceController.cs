using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Repair
{
    public interface IToolInventory
    {
        bool HasTool(ToolDefinition tool);
        void ConsumeTool(ToolDefinition tool);
    }


    [DisallowMultipleComponent]
    public sealed class RepairSequenceController : MonoBehaviour
    { 
        [SerializeField] private SimpleToolInventory inventory;
        [SerializeField] private interactor interactorToLock;
        [SerializeField] private Collider triggerCollider;
        [SerializeField] private float fixTime;


        public UnityEvent<RepairStepTarget> OnStepCompleted;
        public UnityEvent OnSequenceCompleted;
        public UnityEvent<ToolDefinition> OnRequiredToolMissing;
        public UnityEvent OnRepairLeave;

        public IToolInventory Inventory => inventory;

        public Collider TriggerCollider => triggerCollider;

        public float FixTime => fixTime;

        private readonly List<RepairStepTarget> targets = new();
        private int completedCount;

        public void RegisterTarget(RepairStepTarget target)
        {
            targets.Add(target);
        }

        public void NotifyStepCompleted(RepairStepTarget target)
        {
            completedCount++;
            OnStepCompleted?.Invoke(target);

            if(completedCount >= targets.Count)
            {
                OnSequenceCompleted?.Invoke();
            }
        }

        public void NotifyMissingTool(ToolDefinition tool)
        {
            OnRequiredToolMissing?.Invoke(tool);
        }

        public void SetInteractorLocked(bool locked)
        {
            interactorToLock.SetLocked(locked);
        }

        public void NotifyLeavingRepairing()
        {
            OnRepairLeave?.Invoke();
        }
    }
}
