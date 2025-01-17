using System.Collections;
using UnityEngine;

public class Elevator : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject model;
    [SerializeField] private GameObject outlineModel;
    [SerializeField] private float maxHeight = 2f;
    [SerializeField] private float moveDuration = 2f;

    private float minHeight;
    private bool isMovingUp = false;
    private bool isMoving = false;

    private void Start()
    {
        minHeight = transform.position.y;
        Debug.Log(minHeight);
    }

    public void Interact()
    {
        if (!isMoving)
        {
            Debug.Log("Interacting with elevator!");
            StartCoroutine(MoveElevator());
        }
    }

    private IEnumerator MoveElevator()
    {
        isMoving = true;

        Vector3 startPosition = transform.position;
        Vector3 targetPosition = new Vector3(transform.position.x, isMovingUp ? minHeight : maxHeight, transform.position.z);

        float elapsedTime = 0f;

        while (elapsedTime < moveDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / moveDuration;
            t = Mathf.SmoothStep(0, 1, t);

            transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            yield return null;
        }

        transform.position = targetPosition;

        isMovingUp = !isMovingUp;
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
