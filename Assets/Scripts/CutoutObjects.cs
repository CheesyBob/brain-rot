using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutoutObjects : MonoBehaviour
{
    private Transform Player;

    [SerializeField]
    private LayerMask wallMask;

    [SerializeField]
    private float fadeSpeed = 2f;

    private Camera mainCamera;

    private void Awake()
    {
        Player = GameObject.Find("PlayerModel").transform;
        
        mainCamera = GetComponent<Camera>();
    }

    private void Update()
    {
        Vector2 cutoutPos = mainCamera.WorldToViewportPoint(Player.position);
        cutoutPos.y /= (Screen.width / Screen.height);

        Vector3 offset = Player.position - transform.position;
        RaycastHit[] hitObjects = Physics.RaycastAll(transform.position, offset, Mathf.Infinity, wallMask);

        for (int i = 0; i < hitObjects.Length; ++i)
        {
            Material[] materials = hitObjects[i].transform.GetComponent<Renderer>().materials;

            for (int m = 0; m < materials.Length; ++m)
            {
                materials[m].SetVector("_CutoutPos", cutoutPos);
                materials[m].SetFloat("_CutoutSize", Mathf.Lerp(materials[m].GetFloat("_CutoutSize"), 0.1f, Time.deltaTime * fadeSpeed));
                materials[m].SetFloat("_FalloffSize", Mathf.Lerp(materials[m].GetFloat("_FalloffSize"), 0.05f, Time.deltaTime * fadeSpeed));
            }
        }
    }
}