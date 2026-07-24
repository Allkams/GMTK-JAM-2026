using UnityEngine;

namespace Repair
{
    public interface IRepairStepBehaviour
    {
        void Initialize(RepairStepData data, Transform target);

        void OnInteractionBegin();

        void OnInteractionTick(Vector2 pointerDelta, float deltaTime);

        void OnInteractionEnd();

        bool IsComplete { get; }

        float Progress { get; }
    }
}
