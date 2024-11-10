using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]

public class ChangeWallMaterialProperities : MonoBehaviour
{
    public Color customTintColor = Color.white;

    private Material uniqueMaterial;

    void OnValidate()
    {
        UpdateMaterialColor();
    }

    void Start()
    {
        UpdateMaterialColor();
    }

    private void UpdateMaterialColor()
    {
        Renderer renderer = GetComponent<Renderer>();

        uniqueMaterial = new Material(renderer.sharedMaterial);
        renderer.material = uniqueMaterial;

        if(uniqueMaterial.HasProperty("_TintColor"))
        {
            uniqueMaterial.SetColor("_TintColor", customTintColor);
        }
    }

    private void OnDestroy()
    {
        DestroyImmediate(uniqueMaterial);
    }
}