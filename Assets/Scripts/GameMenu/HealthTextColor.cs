using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HealthTextColor : MonoBehaviour
{
    private Color originalColor;

    private void Start()
    {
        originalColor = GetComponent<TextMeshProUGUI>().color;
    }

    private void Update()
    {
        float textValue;

        if (float.TryParse(GetComponent<TextMeshProUGUI>().text, out textValue))
        {
            if (textValue > 30f)
            {
                GetComponent<TextMeshProUGUI>().color = originalColor;
            }
            else
            {
                float clampedValue = Mathf.Clamp(textValue, 0f, 30f);
                float fadeAmount = 1f - (clampedValue / 30f);

                Color textColor = Color.Lerp(originalColor, Color.red, fadeAmount);
                GetComponent<TextMeshProUGUI>().color = textColor;
            }
        }
    }
}