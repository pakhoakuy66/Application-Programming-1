using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class chuyendong : MonoBehaviour
{
    public Slider slider;
    public float movementRange = 10f;
    private RectTransform imageRectTransform;

    private void Start()
    {
        imageRectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        float sliderValue = slider.value;
        float newPositionX = Mathf.Lerp(-movementRange, movementRange, sliderValue);

        Vector3 newPosition = imageRectTransform.localPosition;
        newPosition.x = newPositionX;

        imageRectTransform.localPosition = newPosition;
    }
}
