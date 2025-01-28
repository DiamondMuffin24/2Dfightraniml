using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class PlayerSelectIcon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private Vector3 originalScale;
    private Vector3 originalPosition; // Store the original position
    public Vector3 enlargedScale = new Vector3(1.5f, 1.5f, 1.5f);
    public Vector3 fullSizeScale = new Vector3(8f, 8f, 8f);
    public PlayerSelectionManager selectionManager;
    private bool isSelected = false;

    void Start()
    {
        originalScale = transform.localScale;
        originalPosition = transform.position; // Save original position
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isSelected)
            transform.localScale = enlargedScale;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!isSelected)
            transform.localScale = originalScale;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isSelected) return; // Prevent re-selecting

        isSelected = true;
        selectionManager.SelectCharacter(this);
        StartCoroutine(MoveAndExpand());
    }

    private IEnumerator MoveAndExpand()
    {
        float duration = 0.5f;
        float time = 0f;
        Vector3 startScale = transform.localScale;
        Vector3 startPosition = transform.position;
        Vector3 targetPosition = selectionManager.centerPoint.position;

        while (time < duration)
        {
            float t = time / duration;
            transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            transform.localScale = Vector3.Lerp(startScale, fullSizeScale, t);
            time += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;
        transform.localScale = fullSizeScale;
    }

    public void ResetIcon()
    {
        isSelected = false;
        StartCoroutine(MoveBackToOriginal());
    }

    private IEnumerator MoveBackToOriginal()
    {
        float duration = 0.5f;
        float time = 0f;
        Vector3 startScale = transform.localScale;
        Vector3 startPosition = transform.position;

        while (time < duration)
        {
            float t = time / duration;
            transform.position = Vector3.Lerp(startPosition, originalPosition, t);
            transform.localScale = Vector3.Lerp(startScale, originalScale, t);
            time += Time.deltaTime;
            yield return null;
        }

        transform.position = originalPosition;
        transform.localScale = originalScale;
    }
}
