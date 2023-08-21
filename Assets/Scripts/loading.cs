using UnityEngine.SceneManagement;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class loading : MonoBehaviour
{
    [Header("Menu Screens")]
    [SerializeField]
    private GameObject loadingscreen;

    [SerializeField]
    private GameObject mainmenu;

    [Header("slider")]
    [SerializeField]
    private Slider loadingslider;

    public void loadlevelbtn(string leveltoload)
    {
        mainmenu.SetActive(false);
        loadingscreen.SetActive(true);

        StartCoroutine(loadLevelASync(leveltoload));
    }

    IEnumerator loadLevelASync(string leveltoload)
    {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(leveltoload);

        while (!loadOperation.isDone)
        {
            float progressValue = Mathf.Clamp01(loadingslider.value + Time.deltaTime);
            loadingslider.value = progressValue * 2f;
            yield return null;
        }
    }
}
