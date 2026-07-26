using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LogSequenceSystem : MonoBehaviour
{
    [SerializeField] TextMeshPro LogTextContainer;

    private List<string> LogData = new();

    private string[] LogSteps = {
        "Checking fuses", 
        "Checking spare batteries", 
        "Checking belts on conveyers",
        "Checking temperature sensors",
        "Checking communication bus",
        "Checking calibration data",
        "Checking backup generators",
        "Checking cooling fans"
        };

    int lastStep = -1;

    private float timeTillNextLog;
    private float logTimer;

    private bool Error = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timeTillNextLog = Random.Range(0.6f, 3f);
    }

    // Update is called once per frame
    void Update()
    {
        string LogToDisplay = "";
        foreach(string line in LogData)
        {
            if (line.StartsWith("ERROR"))
            {
                LogToDisplay += $"<color=red>{line}</color>\n";
            }
            else if (line.StartsWith("RESOLVE"))
            {
                LogToDisplay += $"<color=green>{line}</color>\n";
            }
            else
            {
                LogToDisplay += line + "\n";
            }
        }

        LogTextContainer.text = LogToDisplay;

        if(Error)
        {
            return;
        }
        
        logTimer += Time.deltaTime;

        if(logTimer >= timeTillNextLog)
        {
            if(lastStep >= LogSteps.Length)
            {
                lastStep = -1;
            }
            lastStep++;
            LogData.Add(LogSteps[lastStep]);
            if(LogData.Count >= LogSteps.Length)
            {
                LogData.RemoveAt(0);
            }
            timeTillNextLog = Random.Range(0.6f, 3f);
            logTimer = 0f;
        }
    }

    public void CatchError(string errorMessage)
    {
        LogData.Add("ERROR: " + errorMessage);
        if(LogData.Count >= LogSteps.Length)
        {
            LogData.RemoveAt(0);
        }
        Error = true;
    }

    public void SolveError(string solveMessage)
    {
        LogData.Add("RESOLVE: " + solveMessage);
        if(LogData.Count >= LogSteps.Length)
        {
            LogData.RemoveAt(0);
        }
        Error = false;
    }
}
