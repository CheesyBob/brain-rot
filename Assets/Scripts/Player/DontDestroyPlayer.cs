using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyPlayer : MonoBehaviour
{
    private GameObject Player;
    private GameObject PlayerCamera;
    private GameObject PlayerMenu;
    private GameObject[] PlayerChildern;

    private void Awake()
    {
        Player = GameObject.Find("PlayerModel");
        PlayerCamera = GameObject.Find("PlayerCamera");
        PlayerMenu = GameObject.Find("GameMenu");
        PlayerChildern = GameObject.FindGameObjectsWithTag("Player");

        Player.GetComponent<PlayerMovement>().enabled = true;
        PlayerCamera.GetComponent<Camera>().enabled = true;
        PlayerMenu.GetComponent<Canvas>().enabled = true;
        PlayerCamera.GetComponent<AudioListener>().enabled = true;

        foreach(GameObject child in PlayerChildern)
        {
            DontDestroyOnLoad(child.gameObject);
        }
    }
}