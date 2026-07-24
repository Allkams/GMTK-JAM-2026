using Repair;
using UnityEngine;
using UnityEngine.InputSystem;

public class RepairStepTarget : MonoBehaviour, IInteractable
{
    [SerializeField] Renderer meshRenderer;
    [SerializeField] private RepairStepData data;
    [SerializeField] private RepairSequenceController owner;
    [SerializeField] private GameObject visualToHideOnComplete;
    [SerializeField] private RepairStepTarget[] prerequisites;
    [SerializeField] private InputActionReference interactHold;

    private IRepairStepBehaviour behaviour;
    private bool isEngaged; 
    private Material material;

    public bool IsComplete {get; private set;}

    private void Awake()
    {
        owner.RegisterTarget(this);
    }

    private void Start()
    {
        material = meshRenderer.material;
        Highlight(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(!isEngaged)
        {
            return;
        }

        if(!interactHold.action.IsPressed())
        {
            PauseEngagement();
            return;
        }

        Vector2 delta = Mouse.current != null ? Mouse.current.delta.ReadValue() :Vector2.zero;
        behaviour.OnInteractionTick(delta, Time.deltaTime);

        if(behaviour.IsComplete)
        {
            CompleteStep();
        }
    }

    public void Interact()
    {
        if(IsComplete || isEngaged || !PrerequisitesMet())
        {
            return;
        }

        if(data.RequiredTool != null && !owner.Inventory.HasTool(data.RequiredTool))
        {
            owner.NotifyMissingTool(data.RequiredTool);
            return;
        }

        if(behaviour == null)
        {
            behaviour = RepairStepBehaviourFactory.Create(data.StepType);
            behaviour.Initialize(data, transform);
        }

        behaviour.OnInteractionBegin();
        isEngaged = true;
        owner.SetInteractorLocked(true);
    }

    public void Highlight(bool state)
    {
        if(IsComplete || isEngaged || !PrerequisitesMet())
        {
            return;
        }

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

    public string GetPrompt() {return PrerequisitesMet() ? data.Prompt : string.Empty;}

    private bool PrerequisitesMet()
    {
        foreach(RepairStepTarget prerequisite in prerequisites)
        {
            if(!prerequisite.IsComplete)
            {
                return false;
            }
        }
        return true;
    }

    private void PauseEngagement()
    {
        isEngaged = false;
        owner.SetInteractorLocked(false);
    }

    private void CompleteStep()
    {
        behaviour.OnInteractionEnd();
        isEngaged = false;
        IsComplete = true;
        owner.SetInteractorLocked(false);

        if(data.RequiredTool != null && data.RequiredTool.IsConsumedOnUse)
        {
            owner.Inventory.ConsumeTool(data.RequiredTool);
        }

        if(visualToHideOnComplete != null)
        {
            visualToHideOnComplete.SetActive(false);
        }

        owner.NotifyStepCompleted(this);
    }
}
