using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DeathSkullFade : MonoBehaviour
{
    public TextMeshProUGUI healthText;

    private void Update()
    {
        float textValue;

        if (float.TryParse(healthText.text, out textValue))
        {
            float clampedValue = Mathf.Clamp(textValue, 0f, 30f);
            float fadeAmount = 1f - (clampedValue / 30f);

            Color imageColor = GetComponent<Image>().color;
            imageColor.a = fadeAmount;
            GetComponent<Image>().color = imageColor;
        }
    }
}