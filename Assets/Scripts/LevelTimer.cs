using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelTimer : MonoBehaviour
{
    public float timeUntilFail = 100f;

    private float startTime = 0f;
    [SerializeField] private bool hasFinished = false;

    public BaseEvent failedLevelEvent;

    public Slider slider;
    public TextMeshProUGUI timerText;

    private void Start() {
        startTime = Time.time;
    }

    private void UpdateSlider() {
        if(!hasFinished) {
            slider.normalizedValue = (Time.time - startTime) / timeUntilFail;
            timerText.text = ((int)(timeUntilFail - (Time.time - startTime))).ToString();
        }
    }

    private void Update() {
        UpdateSlider();

        // Check if has failed
        if (!hasFinished && Time.time - startTime >= timeUntilFail) {

            failedLevelEvent.Raise();
            hasFinished = true;
            slider.gameObject.SetActive(false);
            timerText.transform.parent.gameObject.SetActive(false);
            Debug.Log("Completed level with " + (Time.time - startTime) + " seconds");
        }
    }

    public void CompletedLevel() {
        hasFinished = true;
        slider.gameObject.SetActive(false);
        timerText.transform.parent.gameObject.SetActive(false);

        Debug.Log("Completed level with " + (Time.time - startTime) + " seconds");
    }
}
