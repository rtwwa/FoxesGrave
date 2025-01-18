using System;
using UnityEngine;
// хз, недописал еще
public class ButtonSequenceManager : MonoBehaviour
{
    [SerializeField] private Button[] buttons;
    [SerializeField] private int[] rightSequence;

    public Action onCompleteSequence { get; set; }

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
            Debug.Log($"Ghost sequence progress: {currentStep}/{rightSequence.Length}");

            if (currentStep >= rightSequence.Length)
            {
                Debug.Log("Ghost sequence completed!");
                onSequenceComplete();
            }
        }
        else
        {
            Debug.Log("Wrong button in ghost mode! Resetting sequence.");
            currentStep = 0;
        }
    }

    private void onSequenceComplete()
    {
        Debug.Log("Puzzle solved...");
        onCompleteSequence?.Invoke();
    }
}
