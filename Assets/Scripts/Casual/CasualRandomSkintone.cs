using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CasualRandomSkintone : MonoBehaviour
{
    public SkinnedMeshRenderer[] skinnedMeshRenderers;

    private int materialIndex1 = 0;

    private void Start()
    {
        ApplyConsistentSkinTones();
    }

    private void ApplyConsistentSkinTones()
    {
        if (skinnedMeshRenderers.Length == 0) return;

        Color skinTone1 = GetRandomSkinTone();

        foreach (SkinnedMeshRenderer renderer in skinnedMeshRenderers)
        {
            Material[] materials = renderer.materials;

            if (materials.Length > materialIndex1)
            {
                materials[materialIndex1].color = skinTone1;
            }
        }
    }

    private Color GetRandomSkinTone()
    {
        float r = Random.Range(0.6f, 0.8f);
        float g = Random.Range(0.4f, 0.6f);
        float b = Random.Range(0.3f, 0.5f);

        float averageBrightness = (r + g + b) / 3.0f;
        if (averageBrightness > 0.75f)
        {
            r *= 0.75f / averageBrightness;
            g *= 0.75f / averageBrightness;
            b *= 0.75f / averageBrightness;
        }
        return new Color(r, g, b);
    }
}