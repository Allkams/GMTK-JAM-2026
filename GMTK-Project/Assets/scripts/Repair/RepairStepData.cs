using UnityEngine;
namespace Repair
{
    [CreateAssetMenu(fileName = "RepairStepData", menuName = "Repair/RepairStepData")]
    public class RepairStepData : ScriptableObject
    {
        [SerializeField] ERepairStepType stepType;
        [SerializeField] ToolDefinition requiredTool;
        [SerializeField] string prompt = "Hold to repair";

        [Header("Rotate")]
        [SerializeField] float rotateTargetDegrees = 720f;
        [SerializeField] Vector3 rotateAxisLocal = Vector3.forward;

        [Header("Linear / Drag / Inser / Press")]
        [SerializeField] Vector3 linearAxisLocal = Vector3.forward;
        [SerializeField] float linearTargetDistance = 0.15f;
        [SerializeField] bool linearInverted;

        public ERepairStepType StepType => stepType;
        public ToolDefinition RequiredTool => requiredTool;
        public string Prompt => prompt;
        public float RotateTargetDegrees => rotateTargetDegrees;

        public Vector3 RotateAxisLocal => rotateAxisLocal;

        public Vector3 LinearAxisLocal => linearAxisLocal;

        public float LinearTargetDistance => linearTargetDistance;

        public bool LinearInverted => linearInverted;
        
    }
}