using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class mainmenu : MonoBehaviour
{
   
     void Start()
    {
       
    }
    public void ExitButton()
    {
        Application.Quit();
        Debug.Log("Game closed");

    }
    public void StartGame()

    {
       
        //SceneManager.LoadScene("Level1");
        
    }
    public void exidgame()
    {
        Application.Quit();
    }
   public void OffMusic()
    {
       
    }
}
