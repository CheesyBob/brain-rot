using UnityEngine;

public class LevelCameraValues : MonoBehaviour
{
    public float camDistance;
    public float camHeight;

    void Awake(){
        GameObject.Find("PlayerCamera").GetComponent<PlayerCamera>().distance = camDistance;
        GameObject.Find("PlayerCamera").GetComponent<PlayerCamera>().height = camHeight;
    }
}