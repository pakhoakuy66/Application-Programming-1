using UnityEngine;
using UnityEngine.SceneManagement;

public class ToNextLevel : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.TryGetComponent<PlayerController>(out _))
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
   
}
