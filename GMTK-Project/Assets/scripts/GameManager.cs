using System.Linq;
using Repair;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public enum State
    {
        RunningSequence,
        Waiting
    }
    [SerializeField] RepairSequenceController[] controllers;
    [SerializeField] TextMeshPro TimeText;

    public UnityEvent<string> OnNewError;

    public State state = State.Waiting;

    // NOTE: All times are counted in seconds.

    private float timeUntilNextFailure = 15f;

    private float currentTimerElasped = 0f;

    private RepairSequenceController currentSequence;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        for(int i = 0; i < controllers.Length; i++)
        {
            controllers[i].TriggerCollider.enabled = false;
        }

        TimeText.text = "No Errors.";
    }

    // Update is called once per frame
    void Update()
    {
        if(currentSequence != null && currentTimerElasped >= currentSequence.FixTime && state == State.RunningSequence)
        {
            // TODO: Implement a game over sequence;
            print("GAME OVER!");
            SceneManager.LoadSceneAsync(0);
        }

        if(state == State.Waiting && currentTimerElasped >= timeUntilNextFailure)
        {
            // TODO: Start next sequence;
            // TODO: Random a new timeUntilNextFailure
            print("Chill time over!");
            state = State.RunningSequence;
            currentTimerElasped = 0f;
            if(controllers.Length > 0)
            {
                currentSequence = controllers.First();
                currentSequence.TriggerCollider.enabled = true;
            }
            // TODO: Give the sequence a name or problem.
            OnNewError.Invoke("Broken fuse.");
        }

        currentTimerElasped += Time.deltaTime;

        if(state == State.RunningSequence)
        {
            float timeLeft = currentSequence.FixTime - currentTimerElasped;
            int minutes = Mathf.FloorToInt(timeLeft / 60f);
            int seconds = (int)timeLeft - (minutes * 60);
            if(minutes < 0)
            {
                minutes = 0;
            }

            if(seconds < 0)
            {
                seconds = 0;
            }

            string minutesStartZero = minutes >= 10 ? "" : "0";
            string secondsStartZero = seconds >= 10 ? "" : "0";
            TimeText.text = "TIME: " + minutesStartZero + minutes.ToString() + ":" + secondsStartZero + seconds.ToString();
        }

    }


}
