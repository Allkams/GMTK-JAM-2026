using UnityEngine;

public class itemPickUp : MonoBehaviour, IInteractable
{
    [SerializeField] Renderer meshRenderer;

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
