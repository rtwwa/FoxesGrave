using System;
using System.Collections;
using UnityEngine;

public class Button : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject model;
    [SerializeField] private GameObject outlineModel;
    [SerializeField] private float moveDuration = 1f;

    public Action InteractAction { get; set; }

    private bool isMoving = false;

    public void Interact()
    {
        if (!isMoving)
        {
            StartCoroutine(PressButton());
            InteractAction?.Invoke();
        }
    }

    private IEnumerator PressButton()
    {
        isMoving = true;

        Vector3 originalPosition = transform.position;
        Vector3 targetPosition = originalPosition + Vector3.down * 0.1f;

        float elapsedTime = 0f;
        float duration = 0.5f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            t = Mathf.SmoothStep(0, 1, t);

            transform.position = Vector3.Lerp(originalPosition, targetPosition, t);
            yield return null;
        }

        elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            t = Mathf.SmoothStep(0, 1, t);

            transform.position = Vector3.Lerp(targetPosition, originalPosition, t);
            yield return null;
        }

        isMoving = false;
    }

    public void ShowOutline()
    {
        if (!isMoving)
        {
            outlineModel.SetActive(true);
        }
    }

    public void HideOutline()
    {
        outlineModel.SetActive(false);
    }
}
