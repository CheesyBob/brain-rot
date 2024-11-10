using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    public float minIntensity;
    public float maxIntensity;
    public float flickerSpeed;

    private List<Light> flickerLights = new List<Light>();
    private List<Material> flickerMaterials = new List<Material>();
    private List<float> baseIntensities = new List<float>();
    private List<Color> originalColors = new List<Color>();

    private void Start()
    {
        Light[] allLights = FindObjectsOfType<Light>();

        foreach (Light light in allLights)
        {
            if (light.gameObject.CompareTag("FlickerLight"))
            {
                flickerLights.Add(light);
                baseIntensities.Add(light.intensity);

                Renderer renderer = light.GetComponent<Renderer>();
                Material mat = renderer.material;

                mat.EnableKeyword("_EMISSION");
                flickerMaterials.Add(mat);
                originalColors.Add(mat.color);
            }
        }
    }

    private void Update()
    {
        for (int i = 0; i < flickerLights.Count; i++)
        {
            float targetIntensity = Random.Range(minIntensity, maxIntensity);
            flickerLights[i].intensity = Mathf.Lerp(flickerLights[i].intensity, targetIntensity, flickerSpeed * Time.deltaTime);

            float intensityRatio = flickerLights[i].intensity / baseIntensities[i];

            if (i < flickerMaterials.Count)
            {
                Color emissionColor = originalColors[i] * intensityRatio;
                flickerMaterials[i].SetColor("_EmissionColor", emissionColor);

                Color targetColor = Color.Lerp(Color.black, originalColors[i], intensityRatio);
                flickerMaterials[i].SetColor("_Color", targetColor);
            }
        }
    }
}