using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void Play()
    {
        SceneManager.LoadScene("ChooseLevel");
    }

    public void ObserveGame()
    {
        SceneManager.LoadScene("ObserveGame");
    }

    public void BackToTitle()
    {
        SceneManager.LoadScene("IntroScene");
    }

    public void ChooseLevel()
    {
        SceneManager.LoadScene("LevelSetup");
    }
}
