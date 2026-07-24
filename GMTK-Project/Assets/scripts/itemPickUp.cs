using UnityEngine;

public class itemPickUp : MonoBehaviour, IInteractable
{
    [SerializeField] Renderer meshRenderer;
    [SerializeField] Repair.ToolDefinition tool;
    [SerializeField] Repair.SimpleToolInventory inventory;

    private Material material;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        material = meshRenderer.material;
        Highlight(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Interact()
    {
        print("I was pressed");
        if(inventory.TryAdd(tool))
        {
            gameObject.SetActive(false);
        }
    }

    public void Highlight(bool state)
    {
        if(state)
        {
            material.EnableKeyword("_EMISSION");
            material.SetColor("_EmissionColor", Color.yellow * 5f);
        }
        else
        {
            material.DisableKeyword("_EMISSION");
        }
    }

    public string GetPrompt()
    {
        return "Screwdriver";
    }
}
