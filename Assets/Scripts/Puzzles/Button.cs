using System;
using System.Collections;
using UnityEngine;

public class Button : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject model;
    [SerializeField] private GameObject outlineModel;
    [SerializeField] private float moveDuration = 0.5f;
    [SerializeField] private PlayerState canActivate;

    public Action InteractAction { get; set; }

    private bool isMoving = false;

    public void Update()
    {
        if (Player.Instance.playerState != PlayerState.Spirit && this.canActivate == PlayerState.Spirit)
        {
            model.SetActive(false);
            outlineModel.SetActive(false);
        }
        else
        {
            model.SetActive(true);
        }
    }

    public void Interact()
    {
        if (!isMoving && (this.canActivate == PlayerState.Both || this.canActivate == Player.Instance.playerState))
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

        while (elapsedTime < moveDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / moveDuration;
            t = Mathf.SmoothStep(0, 1, t);

            transform.position = Vector3.Lerp(originalPosition, targetPosition, t);
            yield return null;
        }

        elapsedTime = 0f;

        while (elapsedTime < moveDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / moveDuration;
            t = Mathf.SmoothStep(0, 1, t);

            transform.position = Vector3.Lerp(targetPosition, originalPosition, t);
            yield return null;
        }

        isMoving = false;
    }

    public void ShowOutline()
    {
        if (!isMoving && (this.canActivate == PlayerState.Both || this.canActivate == Player.Instance.playerState))
        {
            outlineModel.SetActive(true);
        }
    }

    public void HideOutline()
    {
        outlineModel.SetActive(false);
    }
}
