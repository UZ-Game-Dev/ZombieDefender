using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    //Kursor po przejściu do menu/ekranu śmierci zmienia się na standardowy
    private void Start()
    {
        Cursor.SetCursor(null, Vector2.zero.normalized, CursorMode.ForceSoftware);
    }
    //
    public void ButtonNewGame()
    {
        SaveSystem.isGameLoaded = false;
        SceneManager.LoadScene("_MainScene");
    }

    public void LoadGame()
    {
        SaveSystem.isGameLoaded = true;
        SaveSystem.LoadGame();
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