using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutoutObjects : MonoBehaviour
{
    private Transform Player;

    [SerializeField]
    private float fadeSpeed = 2f;

    private Camera mainCamera;

    private void Awake()
    {
        Player = GameObject.Find("PlayerModel").transform;
        mainCamera = GetComponent<Camera>();
    }

    [System.Obsolete]
    private void Update()
    {
        Vector2 cutoutPos = mainCamera.WorldToViewportPoint(Player.position);
        cutoutPos.y /= (Screen.width / Screen.height);

        Renderer[] renderers = FindObjectsOfType<Renderer>();

        foreach (var renderer in renderers)
        {
            if (renderer.gameObject.layer == LayerMask.NameToLayer("Wall"))
            {
                Material[] materials = renderer.materials;

                for (int m = 0; m < materials.Length; ++m)
                {
                    if (materials[m].HasProperty("_CutoutPos"))
                    {
                        materials[m].SetVector("_CutoutPos", cutoutPos);
                    }

                    if (materials[m].HasProperty("_CutoutSize"))
                    {
                        materials[m].SetFloat("_CutoutSize", Mathf.Lerp(materials[m].GetFloat("_CutoutSize"), 0.1f, Time.deltaTime * fadeSpeed));
                    }

                    if (materials[m].HasProperty("_FalloffSize"))
                    {
                        materials[m].SetFloat("_FalloffSize", Mathf.Lerp(materials[m].GetFloat("_FalloffSize"), 0.05f, Time.deltaTime * fadeSpeed));
                    }
                }
            }
        }
    }
}