using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShotgunColorChange : MonoBehaviour
{
    public TextMeshProUGUI ammoText;

    public Material ShotgunMenuMat;

    void Update(){
        if(ammoText.text != "0"){
            ShotgunMenuMat.color = Color.white;
        }
        if(ammoText.text == "0"){
            ShotgunMenuMat.color = Color.red;
        }
    }
}