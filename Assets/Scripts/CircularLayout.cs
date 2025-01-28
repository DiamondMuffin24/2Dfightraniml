using UnityEngine;

public class CircularLayout : MonoBehaviour
{
    public GameObject[] characterIcons; // Drag and drop character icons here
    public float radius = 200f;         // Radius of the circle

    void Start()
    {
        ArrangeInCircle();
    }

    void ArrangeInCircle()
    {
        int totalIcons = characterIcons.Length;
        float angleStep = 360f / totalIcons; // Angle between each character
        Vector2 center = Vector2.zero;      // Center of the circle

        for (int i = 0; i < totalIcons; i++)
        {
            float angle = i * angleStep * Mathf.Deg2Rad; // Convert to radians
            float x = Mathf.Cos(angle) * radius;
            float y = Mathf.Sin(angle) * radius;

            // Position the character icon
            RectTransform iconTransform = characterIcons[i].GetComponent<RectTransform>();
            iconTransform.anchoredPosition = new Vector2(x, y);
        }
    }
}
