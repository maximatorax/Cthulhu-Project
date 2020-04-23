using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public GameObject OptionsPanel;

    void Awake()
    {
        OptionsPanel.SetActive(false);
    }
    public void Play()
    {
        SceneManager.LoadScene("DevTest");
    }
    public void Quit()
    {
        Application.Quit();
    }

    public void Continue()
    {
        SceneManager.LoadScene("DevTest");
    }

    public void Options()
    {
        OptionsPanel.SetActive(true);
    }

    public void Back()
    {
        OptionsPanel.SetActive(false);
    }
}
