using System;
using UnityEngine;
// хз, недописал еще
public class ButtonSequenceManager : MonoBehaviour
{
    [SerializeField] private Button[] buttons;
    [SerializeField] private int[] rightSequence;

    [SerializeField] private IOnComplete onComplete;

    private int currentStep = 0;

    private void Start()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            int index = i;
            buttons[i].gameObject.GetComponent<Button>().InteractAction = () => OnButtonClicked(index);
        }
    }

    private void OnButtonClicked(int buttonIndex)
    {
        if (buttonIndex == rightSequence[currentStep])
        {
            currentStep++;
            Debug.Log($"Sequence progress: {currentStep}/{rightSequence.Length}");

            if (currentStep >= rightSequence.Length)
            {
                currentStep = 0;
                onSequenceComplete();
            }
        }
        else
        {
            Debug.Log("Wrong button! Resetting sequence.");
            currentStep = 0;
        }
    }

    private void onSequenceComplete()
    {
        Debug.Log("Puzzle solved...");
        onComplete?.OnComplete();
    }
}
