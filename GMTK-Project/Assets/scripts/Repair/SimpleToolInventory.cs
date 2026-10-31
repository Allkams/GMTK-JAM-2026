using System.Collections.Generic;
using System.Linq;
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
        private bool[] placement = {false, false, false};

        public bool TryAdd(ToolDefinition tool)
        {
            if(carriedTools.Count >= capacity)
            {
                return false;
            }

            carriedTools.Add(tool);
            int toolIndex = -1;
            for(int i = 0; i < 3; i++)
            {
                if(!placement[i])
                {
                    toolIndex = i;
                    placement[i] = true;
                    break;
                }
            }
            tool.SetToolSlot(toolIndex);
            images[toolIndex].sprite = tool.Icon;
            OnInventoryChanged?.Invoke(carriedTools);
            return true;
        }
        public void ConsumeTool(ToolDefinition tool)
        {
            int toolSlot = tool.GetToolSlot();
            carriedTools.Remove(tool);
            OnInventoryChanged?.Invoke(carriedTools);
            images[toolSlot].sprite = null;
            placement[toolSlot] = false;
        }

        public bool HasTool(ToolDefinition tool) => carriedTools.Contains(tool);

    }

}
