using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class gameover : MonoBehaviour
{
    public GameObject gameOver;
    private bool isgameover=false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("kill") && !isgameover)
        {
            isgameover=true;
            gameOver.gameObject.SetActive(true);
        }
    }
}
