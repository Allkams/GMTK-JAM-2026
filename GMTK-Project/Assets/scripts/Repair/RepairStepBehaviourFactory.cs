using System;
using UnityEngine;

namespace Repair
{
    public static class RepairStepBehaviourFactory
    {
        public static IRepairStepBehaviour Create(ERepairStepType stepType)
        {
            switch (stepType)
            {
                case ERepairStepType.Rotate:
                    return new RotateStepBehaviour();
                case ERepairStepType.Pull:
                case ERepairStepType.Drag:
                case ERepairStepType.Insert:
                case ERepairStepType.Press:
                    return new LinearStepBehaviour();
                default:
                    throw new ArgumentOutOfRangeException(
                        nameof(stepType), stepType, "No behaviour registered for this step type"
                        );
            }
        }
        
    }
}
