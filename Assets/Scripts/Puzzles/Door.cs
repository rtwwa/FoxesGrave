using System.Collections;
using UnityEngine;

public class Door : OnComplete, IInteractable
{
    [SerializeField] private GameObject model;
    [SerializeField] private GameObject outlineModel;
    [SerializeField] private float moveDuration = 1f;
    [SerializeField] private float rotateAngle = -90f;
    [SerializeField] private bool onlyByEvent = false;

    [SerializeField] private OpenType openType; // Новый enum для типа открытия двери
    [SerializeField] private Collider doorCollider; // Collider двери для получения её размеров

    private bool opened;
    private bool isMoving = false;

    // Enum для типа открытия
    public enum OpenType
    {
        Rotate,  // Открытие путём вращения
        SlideUpDown  // Открытие вверх/вниз
    }

    public void Interact()
    {
        if (!isMoving && !onlyByEvent)
        {
            StartCoroutine(InteractWithDoor());
        }
    }

    private IEnumerator InteractWithDoor()
    {
        isMoving = true;

        Quaternion startRotation = transform.rotation;
        Quaternion targetRotation;

        Vector3 startPosition = transform.position;
        Vector3 targetPosition;

        // Получаем размер коллайдера по оси Y
        float doorHeight = doorCollider.bounds.size.y;

        // Логика в зависимости от типа открытия
        switch (openType)
        {
            case OpenType.Rotate:
                if (!opened)
                {
                    targetRotation = Quaternion.Euler(0f, rotateAngle, 0f);
                }
                else
                {
                    targetRotation = Quaternion.Euler(0f, 0f, 0f);
                }

                float rotationElapsedTime = 0f;

                while (rotationElapsedTime < moveDuration)
                {
                    rotationElapsedTime += Time.deltaTime;
                    float t = rotationElapsedTime / moveDuration;
                    t = Mathf.SmoothStep(0, 1, t);

                    transform.rotation = Quaternion.Lerp(startRotation, targetRotation, t);
                    yield return null;
                }

                transform.rotation = targetRotation;
                break;

            case OpenType.SlideUpDown:
                if (!opened)
                {
                    targetPosition = new Vector3(startPosition.x, startPosition.y - doorHeight, startPosition.z); // Открытие вверх на высоту коллайдера
                }
                else
                {
                    targetPosition = startPosition; // Возвращение в исходную позицию
                }

                float slideElapsedTime = 0f;

                while (slideElapsedTime < moveDuration)
                {
                    slideElapsedTime += Time.deltaTime;
                    float t = slideElapsedTime / moveDuration;
                    t = Mathf.SmoothStep(0, 1, t);

                    transform.position = Vector3.Lerp(startPosition, targetPosition, t);
                    yield return null;
                }

                transform.position = targetPosition;
                break;
        }

        opened = !opened;
        isMoving = false;
    }

    public void ShowOutline()
    {
        if (!isMoving && !onlyByEvent)
        {
            outlineModel.SetActive(true);
        }
    }

    public void HideOutline()
    {
        outlineModel.SetActive(false);
    }

    public override void Invoke()
    {
        StartCoroutine(InteractWithDoor());
    }
}
