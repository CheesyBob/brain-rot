using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ChangeWallMaterialProperities : MonoBehaviour
{
    public Color customTintColor = Color.white;
    public Texture customTexture;

    private Material uniqueMaterial;

    void OnValidate()
    {
        UpdateMaterialProperties();
    }

    void Start()
    {
        UpdateMaterialProperties();
    }

    private void UpdateMaterialProperties()
    {
        Renderer renderer = GetComponent<Renderer>();

        uniqueMaterial = new Material(renderer.sharedMaterial);
        renderer.material = uniqueMaterial;

        if (uniqueMaterial.HasProperty("_TintColor"))
        {
            uniqueMaterial.SetColor("_TintColor", customTintColor);
        }

        if (uniqueMaterial.HasProperty("_MainTexture"))
        {
            uniqueMaterial.SetTexture("_MainTexture", customTexture);
        }
    }

    private void OnDestroy()
    {
        if (uniqueMaterial != null)
        {
            DestroyImmediate(uniqueMaterial);
        }
    }
}