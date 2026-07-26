using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Repair
{
    public class SimpleToolInventory : MonoBehaviour, IToolInventory
    {
        [SerializeField] int capacity = 3;
        [SerializeField] List<Image> images = new(3);
        
        public UnityEvent<List<ToolDefinition>> OnInventoryChanged;

        private readonly List<ToolDefinition> carriedTools = new();

        public bool TryAdd(ToolDefinition tool)
        {
            if(carriedTools.Count >= capacity)
            {
                return false;
            }

            carriedTools.Add(tool);
            images[carriedTools.Count - 1].sprite = tool.Icon;
            OnInventoryChanged?.Invoke(carriedTools);
            return true;
        }
        public void ConsumeTool(ToolDefinition tool)
        {
            carriedTools.Remove(tool);
            OnInventoryChanged?.Invoke(carriedTools);
        }

        public bool HasTool(ToolDefinition tool) => carriedTools.Contains(tool);

    }

}
