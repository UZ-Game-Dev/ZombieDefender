using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public GameObject buttonLoadGame;

    private void Start()
    {
        Cursor.SetCursor(null, Vector2.zero.normalized, CursorMode.ForceSoftware);

        if (!File.Exists(Application.persistentDataPath + "/save01.save")) buttonLoadGame.GetComponent<Button>().interactable = false;
    }

    public void ButtonNewGame(GameObject blackBackgroundPanel)
    {
        SaveSystem.isGameLoaded = false;
        blackBackgroundPanel.SetActive(true);
        SceneManager.LoadScene("_MainScene");
    }

    public void LoadGame(GameObject blackBackgroundPanel)
    {
        SaveSystem.isGameLoaded = true;
        SaveSystem.LoadGame();
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

    public void ButtonBackToMenu(GameObject blackBackgroundPanel)
    {
        blackBackgroundPanel.SetActive(true);
        SceneManager.LoadScene("MenuScene");
    }
}