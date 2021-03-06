using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthScript : MonoBehaviour
{
    [SerializeField] private SimpleFloat healthObject = default;
    [SerializeField] private Slider slider;

    private float lastHealthValue;
    //private Image image;

    private void Start()
    {
        healthObject.value = lastHealthValue = 1;
        //image = GetComponent<Image>();
    }

    private void Update()
    {
        if (lastHealthValue != healthObject.value)
        {
            lastHealthValue = healthObject.value;
            slider.value = lastHealthValue;
            //image.color = Color.HSVToRGB(0, 0, lastHealthValue);
        }
    }
}
