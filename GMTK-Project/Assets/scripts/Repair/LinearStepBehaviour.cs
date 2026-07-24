using UnityEngine;

namespace Repair
{
    public sealed class LinearStepBehaviour : IRepairStepBehaviour
    {
        private const float unitsPerPointerUnit = 0.04f;
        
        private RepairStepData data;
        private Transform target;
        private Vector3 startLocalPosition;
        private float accumulatedDistance;


        public bool IsComplete { get; private set; }

        public float Progress { get; private set; }

        public void Initialize(RepairStepData data, Transform target)
        {
            this.data = data;
            this.target = target;

            startLocalPosition = target.localPosition;
            accumulatedDistance = 0f;
            IsComplete = false;
            Progress = 0f;
        }

        public void OnInteractionBegin() {}

        public void OnInteractionEnd() {}

        public void OnInteractionTick(Vector2 pointerDelta, float deltaTime)
        {
            if(IsComplete)
            {
                return;
            }

            float sign = data.LinearInverted ? -1f : 1f;

            float distanceThisFrame = pointerDelta.y * unitsPerPointerUnit * sign;

            accumulatedDistance = Mathf.Clamp(accumulatedDistance + distanceThisFrame, 0f, data.LinearTargetDistance);

            target.localPosition = startLocalPosition + data.LinearAxisLocal.normalized * accumulatedDistance;

            Progress = Mathf.Clamp01(accumulatedDistance / data.LinearTargetDistance);
            IsComplete = Progress >= 1f;
        }
    }

}
