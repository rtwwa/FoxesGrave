using System;
using UnityEngine;
// хз, недописал еще
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

    private int currentStep = 0;       // “екущий шаг в последовательности

    private void Start()
    {
        // Ќазначаем действи€ дл€ кнопок обычного геро€
        for (int i = 0; i < normalHeroButtons.Length; i++)
        {
            int index = i;
            normalHeroButtons[i].gameObject.GetComponent<Button>().InteractAction = () => OnButtonClicked(index);
        }

        // Ќазначаем действи€ дл€ кнопок призрака
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
            // ѕровер€ем последовательность дл€ призрака
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
            // ѕровер€ем последовательность дл€ обычного геро€
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
        // ƒействи€ после завершени€ этапа обычного геро€
        Debug.Log("Switching to ghost mode...");
        currentStep = 0;
    }

    private void OnGhostSequenceComplete()
    {
        // ƒействи€ после завершени€ этапа призрака
        Debug.Log("Puzzle solved as ghost!");
        foreach (var button in ghostHeroButtons)
        {
            button.HideOutline(); // ¬ы можете добавить другие действи€, например, открыть дверь
        }
    }
}
