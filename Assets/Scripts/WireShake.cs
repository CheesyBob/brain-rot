using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WireShake : MonoBehaviour
{
    public float amplitude = 0.1f;
    public float frequency = 1.0f;
    private List<MeshFilter> wireMeshFilters = new List<MeshFilter>();
    private List<Vector3[]> originalVerticesList = new List<Vector3[]>();

    void Start()
    {
        GameObject[] wireObjects = GameObject.FindGameObjectsWithTag("Wire");
        foreach (GameObject wireObject in wireObjects)
        {
            MeshFilter meshFilter = wireObject.GetComponent<MeshFilter>();
            wireMeshFilters.Add(meshFilter);
            originalVerticesList.Add(meshFilter.mesh.vertices);
        }
    }

    void Update()
    {
        for (int i = 0; i < wireMeshFilters.Count; i++)
        {
            MeshFilter meshFilter = wireMeshFilters[i];
            Vector3[] originalVertices = originalVerticesList[i];
            Vector3[] deformedVertices = new Vector3[originalVertices.Length];

            for (int j = 0; j < originalVertices.Length; j++)
            {
                Vector3 vertex = originalVertices[j];
                float offset = Mathf.Sin(Time.time * frequency + vertex.y) * amplitude;
                vertex.x += offset;
                deformedVertices[j] = vertex;
            }
            meshFilter.mesh.vertices = deformedVertices;
            meshFilter.mesh.RecalculateNormals();
        }
    }
}