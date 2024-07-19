using UnityEngine;

public class CasualRandomClothes : MonoBehaviour
{
    [SerializeField]
    private Material[] materials;

    [SerializeField]
    private GameObject[] targetObjects;

    private void Awake()
    {
        int randomIndex = Random.Range(0, materials.Length);
        Material chosenMaterial = materials[randomIndex];

        foreach (GameObject obj in targetObjects)
        {
            Renderer renderer = obj.GetComponent<Renderer>();

            renderer.material = chosenMaterial;
        }
    }
}