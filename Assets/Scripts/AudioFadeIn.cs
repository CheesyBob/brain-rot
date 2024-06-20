using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioFadeIn : MonoBehaviour
{
    private Transform Player;
    public float maxVolumeDistance;
    public float minVolumeDistance;
    public float volumeFadeSpeed;

    private AudioSource audioSource;

    private void Start()
    {
        Player = GameObject.Find("PlayerModel").transform;
        
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, Player.position);
        float targetVolume = Mathf.InverseLerp(minVolumeDistance, maxVolumeDistance, distanceToPlayer);

        audioSource.volume = Mathf.Lerp(audioSource.volume, targetVolume, Time.deltaTime * volumeFadeSpeed);
    }
}