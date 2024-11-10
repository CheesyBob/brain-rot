using UnityEngine;

public class CasualRandomClothes : MonoBehaviour
{
    public SkinnedMeshRenderer[] skinnedMeshRenderers;

    private int materialIndex1 = 0;
    private int materialIndex2 = 1;

    private void Start()
    {
        ApplyConsistentRandomColors();
    }

    private void ApplyConsistentRandomColors()
    {
        if (skinnedMeshRenderers.Length == 0) return;

        Color color1 = GetRandomColor();
        Color color2 = GetRandomColor();

        foreach (SkinnedMeshRenderer renderer in skinnedMeshRenderers)
        {
            Material[] materials = renderer.materials;

            if (materials.Length > materialIndex1 && materials.Length > materialIndex2)
            {
                materials[materialIndex1].color = color1;
                materials[materialIndex2].color = color2;
            }
        }
    }

    private Color GetRandomColor()
    {
        return new Color(Random.value, Random.value, Random.value);
    }
}