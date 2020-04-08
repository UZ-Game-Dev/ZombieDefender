using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    private void Start()
    {
        Cursor.SetCursor(null, Vector2.zero.normalized, CursorMode.ForceSoftware);
    }

    public void ButtonNewGame(GameObject blackBackgroundPanel)
    {
        blackBackgroundPanel.SetActive(true);
        SceneManager.LoadScene("_MainScene");
    }

    public void ButtonOpenPanel(GameObject panel)
    {
        panel.SetActive(true);

    }
    public void ButtonClosePanel(GameObject panel)
    {
        panel.SetActive(false);
    }
    public void ButtonExit(GameObject blackBackgroundPanel)
    {
        blackBackgroundPanel.SetActive(true);
        Application.Quit();
    }

    public void ButtonBackToMenu()
    {
        SceneManager.LoadScene("MenuScene");
    }
}