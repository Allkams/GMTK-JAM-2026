using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Repair
{
    public class SimpleToolInventory : MonoBehaviour, IToolInventory
    {

        [SerializeField] List<Image> images = new();

        
        public UnityEvent<List<ToolDefinition>> OnInventoryChanged;

        private ToolDefinition[] slots;

        private void Awake()
        {
            slots = new ToolDefinition[images.Count];
        }

        public bool TryAdd(ToolDefinition tool)
        {
            int slot = System.Array.IndexOf(slots, null);
            if(slot < 0)
            {
                return false;
            }

            slots[slot] = tool;
            images[slot].sprite = tool.Icon;
            OnInventoryChanged?.Invoke(CarriedTools());
            return true;
        }
        public void ConsumeTool(ToolDefinition tool)
        {
            int slot = System.Array.IndexOf(slots, tool);

            if(slot < 0)
            {
                return;
            }

            slots[slot] = null;
            images[slot].sprite = null;
            OnInventoryChanged?.Invoke(CarriedTools());
        }

        public bool HasTool(ToolDefinition tool) => System.Array.IndexOf(slots, tool ) >= 0;

        private List<ToolDefinition> CarriedTools() => slots.Where(t => t != null).ToList();

    }

}
