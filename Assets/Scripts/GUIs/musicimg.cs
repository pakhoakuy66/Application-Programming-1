using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class musicimg : MonoBehaviour

{
    private Sprite soundonimage;
    public Sprite soundoffimage;
    public Button button;
    private bool ison = true;

    public AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        soundonimage = button.image.sprite;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ButtonClicked()
    {
        if (ison)
        {
            button.image.sprite = soundoffimage;
            ison = false;
            audioSource.mute = true;

        }
        else
        {
            button.image.sprite = soundonimage;
            ison = true;
            audioSource.mute = false;
        }
    }
}
