using System;
using UnityEngine;
// ��, ��������� ���
public class ButtonSequenceManager : MonoBehaviour
{
    [SerializeField] private Button[] normalHeroButtons;
    [SerializeField] private Button[] ghostHeroButtons;
    [SerializeField] private int[] normalSequence;
    [SerializeField] private int[] ghostSequence;

    private bool isCompleteHeroButtons = false;
    private bool isCompleteGhostButtons = false;

    public Action onCompleteHeroButtons { get; set; }
    public Action onCompleteGhostButtons { get; set; }

    private int currentStep = 0;       // ������� ��� � ������������������

    private void Start()
    {
        // ��������� �������� ��� ������ �������� �����
        for (int i = 0; i < normalHeroButtons.Length; i++)
        {
            int index = i;
            normalHeroButtons[i].gameObject.GetComponent<Button>().InteractAction = () => OnButtonClicked(index);
        }

        // ��������� �������� ��� ������ ��������
        for (int i = 0; i < ghostHeroButtons.Length; i++)
        {
            int index = i;
            ghostHeroButtons[i].gameObject.GetComponent<Button>().InteractAction = () => OnButtonClicked(index);
        }
    }

    private void OnButtonClicked(int buttonIndex)
    {
        if (Player.Instance.isSpirit && isCompleteHeroButtons)
        {
            // ��������� ������������������ ��� ��������
            if (buttonIndex == ghostSequence[currentStep])
            {
                currentStep++;
                Debug.Log($"Ghost sequence progress: {currentStep}/{ghostSequence.Length}");

                if (currentStep >= ghostSequence.Length)
                {
                    Debug.Log("Ghost sequence completed!");
                    OnGhostSequenceComplete();
                }
            }
            else
            {
                Debug.Log("Wrong button in ghost mode! Resetting sequence.");
                currentStep = 0;
            }
        }
        else
        {
            // ��������� ������������������ ��� �������� �����
            if (buttonIndex == normalSequence[currentStep])
            {
                currentStep++;
                Debug.Log($"Normal sequence progress: {currentStep}/{normalSequence.Length}");

                if (currentStep >= normalSequence.Length)
                {
                    Debug.Log("Normal sequence completed!");
                    OnNormalSequenceComplete();
                }
            }
            else
            {
                Debug.Log("Wrong button in normal mode! Resetting sequence.");
                currentStep = 0;
            }
        }
    }

    private void OnNormalSequenceComplete()
    {
        // �������� ����� ���������� ����� �������� �����
        Debug.Log("Switching to ghost mode...");
        currentStep = 0;
    }

    private void OnGhostSequenceComplete()
    {
        // �������� ����� ���������� ����� ��������
        Debug.Log("Puzzle solved as ghost!");
        foreach (var button in ghostHeroButtons)
        {
            button.HideOutline(); // �� ������ �������� ������ ��������, ��������, ������� �����
        }
    }
}
