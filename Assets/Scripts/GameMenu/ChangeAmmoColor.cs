using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ChangeAmmoColor : MonoBehaviour
{
    private List<TextMeshProUGUI> ammoTextList = new List<TextMeshProUGUI>();
    private Color originalColor = Color.white;

    void Start()
    {
        GameObject[] ammoTextObjects = GameObject.FindGameObjectsWithTag("AmmoText");
        foreach (GameObject obj in ammoTextObjects)
        {
            TextMeshProUGUI tmp = obj.GetComponent<TextMeshProUGUI>();
            if (tmp != null)
            {
                ammoTextList.Add(tmp);
            }
        }

        if (ammoTextList.Count > 0)
        {
            originalColor = ammoTextList[0].color;
        }
    }

    void Update()
    {
        foreach (TextMeshProUGUI ammoText in ammoTextList)
        {
            if (ammoText.text == "0")
            {
                ammoText.color = Color.red;
            }
            else
            {
                ammoText.color = originalColor;
            }
        }
    }
}