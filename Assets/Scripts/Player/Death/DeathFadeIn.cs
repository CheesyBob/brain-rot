using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeathFadeIn : MonoBehaviour
{
    private Image imageToFade;
    public float fadeInSpeed;
    public float fadeLimit;
    public bool isFading = false;

    private void Start()
    {
        imageToFade = GetComponent<Image>();
        Color imageColor = imageToFade.color;
        imageColor.a = 0.0f;
        imageToFade.color = imageColor;
    }

    private void Update()
    {
        if(isFading)
        {
            Color imageColor = imageToFade.color;
            imageColor.a += fadeInSpeed * Time.deltaTime;
            imageColor.a = Mathf.Clamp(imageColor.a, 0.0f, fadeLimit);
            imageToFade.color = imageColor;
        }
        else
        {
            Color imageColor = imageToFade.color;
            imageColor.a -= fadeInSpeed * Time.deltaTime;
            imageColor.a = Mathf.Clamp(imageColor.a, 0.0f, fadeLimit);
            imageToFade.color = imageColor;
        }
    }
}