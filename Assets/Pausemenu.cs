using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pausemenu : MonoBehaviour
{
    [SerializeField] GameObject PauseMenu;
    public void  Pause()
    {
        PauseMenu.SetActive(true);
        Time.timeScale= 0;
    }
    public void Home()
    {
        SceneManager.LoadScene("HomeScene");
        Time.timeScale = 1;
    }
    public void resume()
    {
        PauseMenu.SetActive(false);
        Time.timeScale = 1;
    }
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
    }
}
