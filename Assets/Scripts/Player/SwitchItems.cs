using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchItems : MonoBehaviour
{
    private Dictionary<KeyCode, GameObject> keyToGameObject = new Dictionary<KeyCode, GameObject>();

    private AudioSource audioSource;
    public AudioClip weaponSwitchSound;

    private GameObject currentActiveObject;

    void Awake(){
        StartCoroutine("UnequipWeapons");
    }

    void Start()
    {
        audioSource = GameObject.Find("PlayerModel").GetComponent<AudioSource>();
        
        keyToGameObject.Add(KeyCode.Alpha1, GameObject.Find("Pistol"));
        keyToGameObject.Add(KeyCode.Alpha2, GameObject.Find("Shotgun"));
        keyToGameObject.Add(KeyCode.Alpha3, GameObject.Find("RocketLauncher"));
        keyToGameObject.Add(KeyCode.Alpha4, GameObject.Find("Flamethrower"));
        keyToGameObject.Add(KeyCode.Alpha5, GameObject.Find("Molotov"));
        keyToGameObject.Add(KeyCode.Alpha6, GameObject.Find("Deathwad"));
    }

    IEnumerator UnequipWeapons(){
        yield return null;
        DeactivateAllGameObjects();
    }

    void Update()
    {
        foreach (var kvp in keyToGameObject)
        {
            if (Input.GetKeyDown(kvp.Key) && !GetComponent<PlayerBurn>().isBurning)
            {
                if (currentActiveObject != null)
                {
                    if (currentActiveObject == kvp.Value)
                    {
                        currentActiveObject.SetActive(false);
                        currentActiveObject = null;
                        audioSource.Stop();
                    }
                    else
                    {
                        currentActiveObject.SetActive(false);
                        kvp.Value.SetActive(true);
                        currentActiveObject = kvp.Value;
                        audioSource.PlayOneShot(weaponSwitchSound);
                    }
                }
                else
                {
                    kvp.Value.SetActive(true);
                    currentActiveObject = kvp.Value;
                    audioSource.Play();
                }
            }
        }
        if(GetComponent<PlayerBurn>().isBurning){
            currentActiveObject.SetActive(false);
        }
    }

    void DeactivateAllGameObjects()
    {
        foreach (var kvp in keyToGameObject)
        {
            kvp.Value.SetActive(false);
        }
    }
}