using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu_Script : MonoBehaviour {

	public static bool IsPaused = false;
    public GameObject PauseMenu;
    public GameObject GameOverMenu;
    public GameObject MuteButton;
	
	void Start () {
        IsPaused = false;
        PauseMenu.SetActive(false);
        GameOverMenu.SetActive(false);
        MuteButton.SetActive(false);
	}

    private void Update()
    {
        if (GameManager_Script.GameOver)
        {
            GameOverMenu.SetActive(true);
            MuteButton.SetActive(true);
        }
    }

    public void Pause()
    {
        PauseMenu.SetActive(true);
        MuteButton.SetActive(true);
        IsPaused = true;
    }

    public void Resume()
    {
        PauseMenu.SetActive(false);
        MuteButton.SetActive(false);
        IsPaused = false;
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
