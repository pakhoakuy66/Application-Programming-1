using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pausemenu : MonoBehaviour
{
   public GameObject PauseMenu;
    private bool isPaused=false;
    public void  Pause()
    {
        PauseMenu.SetActive(true);
        Time.timeScale= 0;
        isPaused = true;
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
        isPaused = false;
    }
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                resume();
            }
            else
            {
                Pause();
            }
        }
    }
}
