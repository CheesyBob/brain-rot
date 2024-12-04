using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCargoMaterialColor : MonoBehaviour
{
    [Range(0.0f, 1.0f)]
    private float maxBrightness = 0.4f;

    void Start()
    {
        GameObject[] allObjects = FindObjectsOfType<GameObject>();
        List<GameObject> cargos = new List<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            if (obj.name == "Cargo")
            {
                cargos.Add(obj);
            }
        }

        List<Color> usedColors = new List<Color>();

        foreach (GameObject cargo in cargos)
        {
            Renderer cargoRenderer = cargo.GetComponent<Renderer>();
            Material cargoMaterial = cargoRenderer.material;
            Color randomColor;

            do
            {
                randomColor = new Color(
                    Random.Range(0.0f, maxBrightness),
                    Random.Range(0.0f, maxBrightness),
                    Random.Range(0.0f, maxBrightness)
                );
            }
                while (usedColors.Contains(randomColor));

                usedColors.Add(randomColor);

                cargoMaterial.SetColor("_BaseColor", randomColor);
        }
    }
}