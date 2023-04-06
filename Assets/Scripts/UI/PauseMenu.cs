using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    private GameObject pauseMenu;
    private void Awake()
    {
        pauseMenu = GameObject.FindWithTag("UI").transform.Find("PauseMenu").gameObject;
    }
    public void setPause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0;
    }

    public void resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }

    public void menu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }
    public void restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Game");
    }
}
