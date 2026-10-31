using Repair;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class RepairStepTarget : MonoBehaviour, IInteractable
{
    private enum Phase
    {
        AwaitingPrimary,
        AwaitingReverse,
        Complete
    }
    [SerializeField] Renderer meshRenderer;
    [SerializeField] private RepairStepData primaryData;
    [SerializeField] private RepairStepData reverseData;
    [SerializeField] private RepairSequenceController owner;
    [SerializeField] private RepairStepTarget[] prerequisites;
    [SerializeField] private RepairStepTarget[] reversePrerequisites;
    [SerializeField] private InputActionReference interactHold;

    public UnityEvent OnPrimaryCompleted;
    public UnityEvent OnReverseCompleted;
    public UnityEvent OnReset;

    private Phase phase = Phase.AwaitingPrimary;
    private IRepairStepBehaviour behaviour;
    private bool isEngaged; 

    private Vector3 resetLocalPosition;
    private Quaternion resetLocalRotatiion;
    private Material material;

    public bool HasCompletedPrimary => phase != Phase.AwaitingPrimary;
    public bool IsFullyComplete => phase == Phase.Complete;

    private RepairStepData CurrentData => phase == Phase.AwaitingPrimary ? primaryData : reverseData;

    private void Awake()
    {
        resetLocalPosition = transform.localPosition;
        resetLocalRotatiion = transform.rotation;
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
            CompletePhase();
        }
    }

    public void Interact()
    {
        print(PrerequisitesMet());
        if(phase == Phase.Complete || isEngaged || !PrerequisitesMet())
        {
            return;
        }

        RepairStepData data = CurrentData;
        print("Data: " + data.name);

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
        if(phase == Phase.Complete || isEngaged || !PrerequisitesMet())
        {
            material.DisableKeyword("_EMISSION");
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

    public string GetPrompt() {return (phase != Phase.Complete && PrerequisitesMet()) ? CurrentData.Prompt : string.Empty;}

    public void ResetStep()
    {
        phase = Phase.AwaitingPrimary;
        behaviour = null;
        isEngaged = false;
        transform.SetLocalPositionAndRotation(resetLocalPosition, resetLocalRotatiion);
        OnReset?.Invoke();
    }

    private bool PrerequisitesMet()
    {
        RepairStepTarget[] required = phase == Phase.AwaitingPrimary ? prerequisites : reversePrerequisites;

        foreach(RepairStepTarget requirement in required)
        {
            bool met = phase == Phase.AwaitingPrimary ? requirement.HasCompletedPrimary : requirement.IsFullyComplete; 

            if(!met)
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

    private void CompletePhase()
    {
        RepairStepData finishedData = CurrentData;
        behaviour.OnInteractionEnd();
        behaviour = null;
        isEngaged = false;
        owner.SetInteractorLocked(false);

        if(finishedData.RequiredTool != null && finishedData.RequiredTool.IsConsumedOnUse)
        {
            owner.Inventory.ConsumeTool(finishedData.RequiredTool);
        }

        if(phase == Phase.AwaitingPrimary)
        {
            phase = reverseData != null ? Phase.AwaitingReverse : Phase.Complete;
            OnPrimaryCompleted?.Invoke();
        }
        else
        {
            phase = Phase.Complete;
            OnReverseCompleted?.Invoke();
        }

        if(phase == Phase.Complete)
        {
            owner.NotifyStepCompleted(this);
        }

    }
}
