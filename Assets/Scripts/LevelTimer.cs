using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTimer : MonoBehaviour
{
    public float timeUntilFail = 100f;

    private float startTime = 0f;
    [SerializeField] private bool hasFinished = false;

    public BaseEvent failedLevelEvent;

    private void Start() {
        startTime = Time.time;
    }

    private void Update() {
        if (!hasFinished && Time.time - startTime >= timeUntilFail) {
            failedLevelEvent.Raise();
            hasFinished = true;
            Debug.Log("Completed level with " + (Time.time - startTime) + " seconds");
        }
    }

    public void CompletedLevel() {
        hasFinished = true;

        Debug.Log("Completed level with " + (Time.time - startTime) + " seconds");
    }
}
