using UnityEngine;
using System.Collections;

public class FlyInAndShake : MonoBehaviour
{
    public enum FlyDirection { Left, Right, Top, Bottom }

    private FlyDirection flyDirection = FlyDirection.Right; // Direction to fly in from
    public float flyInDuration = 1f; // Duration of flying in
    public float shakeDuration = 0.3f; // Duration of shaking
    public float shakeAmount = 10f; // How much to shake
    public float settleDuration = 0.5f; // Time for UI element to settle
    public AudioClip soundEffect; // Sound effect to play
    public float soundTriggerTime = 0.5f; // When the sound effect should play during the animation (0 = start, 1 = end)
    public float soundPitch = 1f; // Pitch of the sound effect (individual pitch per UI element)

    private float totalDuration;
    private float elapsedTime;

    private RectTransform rectTransform;
    private Vector2 initialPosition;
    private Vector2 offScreenPosition;
    private Vector2 targetPosition;
    private AudioSource audioSource; // Reference to the AudioSource component for each individual UI element

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();

        // If there's no AudioSource attached to the GameObject, add one
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        initialPosition = rectTransform.anchoredPosition;
        targetPosition = initialPosition;

        // Set the off-screen position based on the selected direction
        SetOffScreenPosition();

        rectTransform.anchoredPosition = offScreenPosition; // Start off-screen
        StartCoroutine(FlyInShakeAndSettle());
    }

    void SetOffScreenPosition()
    {
        // Calculate the off-screen position based on the fly direction
        switch (flyDirection)
        {
            case FlyDirection.Left:
                offScreenPosition = new Vector2(-Screen.width, initialPosition.y);
                break;
            case FlyDirection.Right:
                offScreenPosition = new Vector2(Screen.width, initialPosition.y);
                break;
            case FlyDirection.Top:
                offScreenPosition = new Vector2(initialPosition.x, Screen.height);
                break;
            case FlyDirection.Bottom:
                offScreenPosition = new Vector2(initialPosition.x, -Screen.height);
                break;
        }
    }

    IEnumerator FlyInShakeAndSettle()
    {
        // Fly in
        totalDuration = flyInDuration + shakeDuration + settleDuration;
        elapsedTime = 0f;

        // Fly in phase
        while (elapsedTime < flyInDuration)
        {
            rectTransform.anchoredPosition = Vector2.Lerp(offScreenPosition, targetPosition, elapsedTime / flyInDuration);
            elapsedTime += Time.deltaTime;
            PlaySoundIfTimeReached(elapsedTime, totalDuration);
            yield return null;
        }
        rectTransform.anchoredPosition = targetPosition; // Ensure it exactly reaches the target

        // Shake phase
        while (elapsedTime < flyInDuration + shakeDuration)
        {
            rectTransform.anchoredPosition = targetPosition + Random.insideUnitCircle * shakeAmount;
            elapsedTime += Time.deltaTime;
            PlaySoundIfTimeReached(elapsedTime, totalDuration);
            yield return null;
        }

        // Settle phase
        while (elapsedTime < totalDuration)
        {
            rectTransform.anchoredPosition = Vector2.Lerp(rectTransform.anchoredPosition, targetPosition, (elapsedTime - (flyInDuration + shakeDuration)) / settleDuration);
            elapsedTime += Time.deltaTime;
            PlaySoundIfTimeReached(elapsedTime, totalDuration);
            yield return null;
        }
        rectTransform.anchoredPosition = targetPosition; // Ensure it settles at the target
    }

    // Plays sound effect when the current time exceeds the set soundTriggerTime (scaled by total animation time)
    void PlaySoundIfTimeReached(float elapsedTime, float totalDuration)
    {
        if (soundEffect != null && audioSource != null && elapsedTime >= soundTriggerTime * totalDuration)
        {
            audioSource.pitch = soundPitch; // Set the pitch for this UI element's AudioSource
            audioSource.PlayOneShot(soundEffect);
            soundEffect = null; // Prevent it from playing multiple times
        }
    }
}
