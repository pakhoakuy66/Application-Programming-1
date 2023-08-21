using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class gameovern : MonoBehaviour
{
    [SerializeField] GameObject Pauseover;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
    }
    public void Home()
    {
        SceneManager.LoadScene("HomeScene");
        Time.timeScale = 1;
    }
}
