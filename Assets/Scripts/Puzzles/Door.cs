using System.Collections;
using UnityEngine;

public class Door : OnComplete, IInteractable
{
    [SerializeField] private GameObject model;
    [SerializeField] private GameObject outlineModel;
    [SerializeField] private float moveDuration = 1f;
    [SerializeField] private float rotateAngle = -90f;
    [SerializeField] private bool onlyByEvent = false;
    private Quaternion startRotation;

    [SerializeField] private OpenType openType; // ����� enum ��� ���� �������� �����
    [SerializeField] private Collider doorCollider; // Collider ����� ��� ��������� � ��������

    private bool opened;
    private bool isMoving = false;

    private void Start()
    {
        startRotation = transform.rotation;
    }

    // Enum ��� ���� ��������
    public enum OpenType
    {
        Rotate,  // �������� ���� ��������
        SlideUpDown  // �������� �����/����
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

        Quaternion targetRotation;

        Vector3 startPosition = transform.position;
        Vector3 targetPosition;

        // �������� ������ ���������� �� ��� Y
        float doorHeight = doorCollider.bounds.size.y;

        // ������ � ����������� �� ���� ��������
        switch (openType)
        {
            case OpenType.Rotate:
                if (!opened)
                {
                    targetRotation = Quaternion.Euler(0f, startRotation.eulerAngles.y + rotateAngle, 0f);
                }
                else
                {
                    targetRotation = startRotation;
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
                    targetPosition = new Vector3(startPosition.x, startPosition.y - doorHeight, startPosition.z); // �������� ����� �� ������ ����������
                }
                else
                {
                    targetPosition = startPosition; // ����������� � �������� �������
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
