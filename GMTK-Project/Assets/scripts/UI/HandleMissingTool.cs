using Repair;
using TMPro;
using UnityEngine;

public class HandleMissingTool : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI promptText;

    private string textToDisplay = "";

    private float timeToDisplay = 15f;
    private float ticker = 0f;

    void Update()
    {
        if(textToDisplay == "" && promptText.text == "")
        {
            return;
        }

        promptText.text = textToDisplay;

        ticker += Time.deltaTime;
        if(ticker >= timeToDisplay)
        {
            textToDisplay = "";
            ticker = 0f;
        }
    }

    public void PromptMissingTool(ToolDefinition tool)
    {
        textToDisplay = "MISSING TOOL: " + tool.DisplayName;
        ticker = 0f;
    }
}
