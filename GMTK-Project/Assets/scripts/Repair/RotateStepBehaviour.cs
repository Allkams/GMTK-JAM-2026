using Repair;
using UnityEngine;

namespace Repair
{
    public sealed class RotateStepBehaviour : IRepairStepBehaviour
    {
        private const float DegreesPerPointerUnit = 10f;

        private RepairStepData data;
        private Transform target;
        private float accumulatedDegrees;


        public bool IsComplete {get; private set;}
        public float Progress {get; private set;}

        public void Initialize(RepairStepData data, Transform target)
        {
            this.data = data;
            this.target = target;
            accumulatedDegrees = 0f;
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

            float degreesThisFrame = pointerDelta.x * DegreesPerPointerUnit;
            accumulatedDegrees += degreesThisFrame;

            target.Rotate(data.RotateAxisLocal, degreesThisFrame, Space.Self);

            Progress = Mathf.Clamp01(Mathf.Abs(accumulatedDegrees) / data.RotateTargetDegrees);
            IsComplete = Progress >= 1f;
        }
    }
}
