using Repair;
using UnityEngine;

namespace Repair
{
    public sealed class RotateStepBehaviour : IRepairStepBehaviour
    {
        private const float DegreesPerPointerUnit = 180f;

        private RepairStepData data;
        private Transform target;
        private float accumulatedDegrees;

        private Vector3 startLocalPosition;


        public bool IsComplete {get; private set;}
        public float Progress {get; private set;}

        public void Initialize(RepairStepData data, Transform target)
        {
            this.data = data;
            this.target = target;
            accumulatedDegrees = 0f;
            startLocalPosition = target.localPosition;
            IsComplete = false;
            Progress = 0f;
        }


        public void OnInteractionBegin()
        {
            
        }

        public void OnInteractionEnd()
        {
            
        }

        public void OnInteractionTick(Vector2 pointerDelta, float deltaTime)
        {
            if(IsComplete)
            {
                return;
            }

            float degreesThisFrame = DegreesPerPointerUnit * deltaTime;
            accumulatedDegrees += degreesThisFrame;

            target.Rotate(data.RotateAxisLocal, degreesThisFrame, Space.Self);

            float sign = data.LinearInverted ? -1f : 1f;

            Progress = Mathf.Clamp01(Mathf.Abs(accumulatedDegrees) / data.RotateTargetDegrees);
            Vector3 pos = startLocalPosition + data.LinearAxisLocal * data.LinearTargetDistance * sign;
            target.localPosition = Vector3.Lerp(startLocalPosition,  pos, Progress);
            IsComplete = Progress >= 1f;
        }
    }
}
