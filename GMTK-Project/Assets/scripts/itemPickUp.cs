using System.Collections.Generic;
using UnityEngine;

public class itemPickUp : MonoBehaviour, IInteractable
{
    [SerializeField] Renderer[] meshRenderers;
    [SerializeField] Repair.ToolDefinition tool;
    [SerializeField] Repair.SimpleToolInventory inventory;

    private List<Material> materials = new();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for(int i = 0; i < meshRenderers.Length; i++)
        {
            materials.Add(meshRenderers[i].material);
        }
        Highlight(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Interact()
    {
        if(inventory.TryAdd(tool))
        {
            gameObject.SetActive(false);
        }
    }

    public void Highlight(bool state)
    {
        if(state)
        {
            foreach(var material in materials)
            {
                material.EnableKeyword("_EMISSION");
                material.SetColor("_EmissionColor", Color.yellow * 5f);
            }
        }
        else
        {
            foreach(var material in materials)
            {
                material.DisableKeyword("_EMISSION");
            }  
        }
    }

    public string GetPrompt()
    {
        return "Press E to pick up " + tool.DisplayName;
    }
}
