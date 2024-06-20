using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerSpawn : MonoBehaviour
{
    private CharacterController player;

    void Awake()
    {
        player = FindObjectOfType<CharacterController>();
    }

    void Start()
    {
        player.enabled = false;
        player.transform.position = transform.position;
        player.enabled = true;
    }
}