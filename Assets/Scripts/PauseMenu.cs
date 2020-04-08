using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu S;

    [SerializeField] bool _isPaused;

    public GameObject pauseMenuUI;

    //dodanie UI zapytania o potwierdzenie czy wyjść z gry
    public GameObject infoExitUI;
    public GameObject blackBackground;
    //

    void Awake()
    {
        if (S != null)
            Debug.LogError("Sigleton Pausemenu juz istnieje");
        S = this;
    }

    public bool GetIsPaused()
    {
        return _isPaused;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) _isPaused = !_isPaused;

        if (_isPaused) ActivateMenu();
        else DeactivateMenu();
    }

    private void ActivateMenu()
    {
        Time.timeScale = 0;
        //AudioListener.pause = true; <- linijka do usunięcia
        pauseMenuUI.SetActive(true);
    }

    public void DeactivateMenu()
    {
        Time.timeScale = 1;
        //AudioListener.pause = false; <- linijka do usunięcia
        // Zamyka okno zapytania po wciśnięciu Esc
        infoExitUI.SetActive(false);
        pauseMenuUI.SetActive(false);
        _isPaused = false;
    }

    public void ButtonBackToMenu()
    {
        //Time.timeScale = 1f; < -linijka do usunięcia
        //SceneManager.LoadScene("MenuScene"); < -linijka do usunięcia
        //zmiana, teraz otwiera się menu zapytania o potwierdzenie czy wyjść z gry
        infoExitUI.SetActive(true);
        //
    }

    //dodanie przycisków menu zapytania o potwierdzenie czy wyjść z gry
    public void ButtonYes()
    {
        blackBackground.SetActive(true);
        Time.timeScale = 1f;
        SceneManager.LoadScene("MenuScene");
    }

    public void ButtonNo()
    {
        infoExitUI.SetActive(false);
    }
    //
}