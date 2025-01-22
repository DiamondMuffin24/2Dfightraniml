using UnityEngine;
using UnityEngine.UI; // For accessing the legacy UI components

public class CountdownTimer : MonoBehaviour
{
    public Text countdownText; // Reference to the legacy Text component
    public float startTime = 10f; // Countdown start time in seconds
    private float timeRemaining;

    private bool isCountingDown = true;

    void Start()
    {
        if (countdownText == null)
        {
            Debug.LogError("Countdown Text is not assigned in the inspector!");
            isCountingDown = false;
        }

        timeRemaining = startTime;
        UpdateCountdownText();
    }

    void Update()
    {
        if (isCountingDown && timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            timeRemaining = Mathf.Max(timeRemaining, 0); // Clamp to zero
            UpdateCountdownText();

            if (timeRemaining <= 0)
            {
                OnCountdownEnd();
            }
        }
    }

    void UpdateCountdownText()
    {
        // Only display the remaining time as an integer
        countdownText.text = Mathf.CeilToInt(timeRemaining).ToString();
    }

    void OnCountdownEnd()
    {
        isCountingDown = false;
        countdownText.text = "0"; // Ensure it displays 0 at the end
        Debug.Log("Countdown Finished!");
    }
}
