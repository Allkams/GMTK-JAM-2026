using UnityEngine;

namespace Repair
{
    [System.Serializable]
    public struct RepairStepBinding
    {
        public RepairStepData data;
        public Transform sceneTarget;
    }
}
