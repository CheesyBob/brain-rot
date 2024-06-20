using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PainFadeIn : MonoBehaviour
{
    private Image imageToFade;

    public float fadeSpeed;
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
        Color imageColor = imageToFade.color;

        if (isFading)
        {
            imageColor.a += fadeSpeed * Time.deltaTime;
        }
        else
        {
            imageColor.a -= fadeSpeed * Time.deltaTime;
        }

        imageColor.a = Mathf.Clamp(imageColor.a, 0.0f, fadeLimit);
        imageToFade.color = imageColor;

        if(isFading){
            StartCoroutine("StopFadding");
        }
    }

    IEnumerator StopFadding(){
        yield return new WaitForSeconds(0.1f);

        isFading = false;
    }
}