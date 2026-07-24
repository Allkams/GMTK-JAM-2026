using UnityEngine;

namespace Repair
{
    [CreateAssetMenu(fileName = "ToolDefinition", menuName = "Repair/ToolDefinition")]
    public sealed class ToolDefinition : ScriptableObject
    {
        [SerializeField] string toolId;
        [SerializeField] string displayName;
        [SerializeField] Sprite icon;
        [SerializeField] bool isConsumedOnUse;

        public string ToolId => toolId;
        public string DisplayName => displayName;
        public Sprite Icon => icon;
        public bool IsConsumedOnUse => isConsumedOnUse;
    }
}
