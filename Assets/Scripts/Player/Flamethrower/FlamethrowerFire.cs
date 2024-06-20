using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FlamethrowerFire : MonoBehaviour
{
    public TextMeshProUGUI textMesh;

    public GameObject FlameThrowerParticles;

    public AudioClip flamethrowerStart;
    public AudioClip flamethrowerEnd;

    public float subtractRate;
    private float nextSubtractionTime = 0f;

    public int subtractValue;

    private bool isMouseDown = false;
    public bool noAmmo = false;

    void Awake(){
        GetComponent<AudioSource>().Stop();
        
        isMouseDown = false;
    }

    void OnDisable(){
        GetComponent<AudioSource>().Stop();

        isMouseDown = false;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if(!noAmmo){
                isMouseDown = true;

                FlameThrowerParticles.GetComponent<ParticleSystem>().Play();

                GetComponent<AudioSource>().volume = 1;
                GetComponent<AudioSource>().loop = true;
                GetComponent<AudioSource>().clip = flamethrowerStart;

                GetComponent<AudioSource>().Play();
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if(!noAmmo){
                isMouseDown = false;

                FlameThrowerParticles.GetComponent<ParticleSystem>().Stop();

                GetComponent<AudioSource>().volume = 1;
                GetComponent<AudioSource>().loop = false;
                GetComponent<AudioSource>().clip = flamethrowerEnd;

                GetComponent<AudioSource>().Play();
            }
        }

        if (isMouseDown && Time.time >= nextSubtractionTime)
        {
            int currentValue = int.Parse(textMesh.text);

            currentValue -= subtractValue;

            currentValue = Mathf.Max(0, currentValue);

            textMesh.text = currentValue.ToString();

            nextSubtractionTime = Time.time + 1f / subtractRate;

            if(currentValue <= 0)
            {
                isMouseDown = false;

                FlameThrowerParticles.GetComponent<ParticleSystem>().Stop();

                noAmmo = true;
            }
        }

        if(textMesh.text == "0"){
            GetComponent<AudioSource>().clip = null;
            GetComponent<AudioSource>().volume = 0;

            noAmmo = true;
        }
        if(textMesh.text != "0"){
            GetComponent<AudioSource>().volume = 1;

            noAmmo = false;
        }
    }
}