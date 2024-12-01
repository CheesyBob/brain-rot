using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonEnableGameObject : MonoBehaviour
{
    public void OnClick(GameObject targetObject)
    {
        targetObject.SetActive(!targetObject.activeSelf);
    }
}