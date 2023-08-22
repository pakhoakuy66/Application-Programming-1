using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DIALOGBOX : MonoBehaviour
{
    public GameObject PanelwithAnimation;
    public GameObject textpanel;
    public Text autoScrolltext;
    public float autoScrollSpeed = 30f;
    public float delayBefore = 2f;

    // Start is called before the first frame update
    private void Start()
    {
        StartCoroutine(ShowAutoScroollText());
    }
    IEnumerator ShowAutoScroollText()
    {
        yield return new WaitForSeconds(delayBefore);

        textpanel.SetActive(true);

        string autoScrollContent = autoScrolltext.text;

        autoScrolltext.text = "";

        for (int i = 0; i <= autoScrollContent.Length; i++)
        {
            autoScrolltext.text = autoScrollContent.Substring(0,i);
            yield return new WaitForSeconds(1f / autoScrollSpeed);
        }

        PanelwithAnimation.SetActive(true);
    }
    // Update is called once per frame
    void Update()
    {
        
    }

   
    
}
