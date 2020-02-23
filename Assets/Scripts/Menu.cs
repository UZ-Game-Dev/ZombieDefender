using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void ButtonNewGame()
    {
        SceneManager.LoadScene("_MainScene");
    }

    public void ButtonCredits(GameObject creditsPanel)
    {
        creditsPanel.SetActive(true);

    }
    public void ButtonCloseCredits(GameObject creditsPanel)
    {
        creditsPanel.SetActive(false);
    }
    public void ButtonExit()
    {
        Application.Quit();
    }

    public void ButtonBackToMenu()
    {
        SceneManager.LoadScene("MenuScene");
    }
}