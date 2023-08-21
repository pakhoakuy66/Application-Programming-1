using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class levelmap : MonoBehaviour
{
    public Button[] buttons;

    private void Awake()
    {
      
    }
    public void openlevel(int levelid)
    {
        string levelname = "level" + levelid;
        SceneManager.LoadScene(levelname);
    }
}
