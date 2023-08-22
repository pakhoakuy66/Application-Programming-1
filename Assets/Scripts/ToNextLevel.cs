using UnityEngine;
using UnityEngine.SceneManagement;

public class ToNextLevel : MonoBehaviour
{
    public GameObject Winmenu;
    private int nextSceneIndex;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.TryGetComponent<PlayerController>(out _))
        {
            Winmenu.SetActive(true);
            Time.timeScale = 0;
        }
           
    }
    public void NextLevel()
    {
        if(nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
            Time.timeScale = 1;
        }
    }
    private void Start()
    {
         nextSceneIndex=SceneManager.GetActiveScene().buildIndex + 1;
    }

}
