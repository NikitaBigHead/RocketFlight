using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Menu : MonoBehaviour
{
    public void game()
    {
        SceneManager.LoadScene("Game");
    }
    public void exit()
    {
        Application.Quit();
    }
}
