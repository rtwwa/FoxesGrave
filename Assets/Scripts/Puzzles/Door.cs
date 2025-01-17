using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject model;
    [SerializeField] private GameObject outlineModel;
    [SerializeField] private float moveDuration = 1f;
    [SerializeField] private float rotateAngle = -90f;


    private bool opened;
    private bool isMoving = false;

    public void Interact()
    {
        if (!isMoving)
        {
            StartCoroutine(InteractWithDoor());
        }
    }

    private IEnumerator InteractWithDoor()
    {
        isMoving = true;

        Quaternion startRotation = transform.rotation;
        Quaternion targetRotation;

        if (!opened)
        {
            targetRotation = Quaternion.Euler(0f, rotateAngle, 0f);
        } 
        else
        {
            targetRotation = Quaternion.Euler(0f, 0f, 0f);
        }

        float elapsedTime = 0f;

        while (elapsedTime < moveDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / moveDuration;
            t = Mathf.SmoothStep(0, 1, t);

            transform.rotation = Quaternion.Lerp(startRotation, targetRotation, t);
            yield return null;
        }

        transform.rotation = targetRotation;

        opened = !opened;
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
