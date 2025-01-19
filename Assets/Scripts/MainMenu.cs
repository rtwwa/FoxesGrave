using System.Collections;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public bool isLoad;

    public GameObject settings;
    public GameObject mainMenu;
    public GameObject mainMenuCamera;

    public void SettingsGame()
    {
        settings.SetActive(!settings.activeSelf);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    void Update()
    {
        if (isLoad == true)
        {
            Destroy(mainMenu);
            Destroy(mainMenuCamera);
        }
    }
}
