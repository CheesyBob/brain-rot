using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AmmoKit : MonoBehaviour
{
    private TextMeshProUGUI shotgunAmmoText;
    private TextMeshProUGUI rocketAmmoText;
    private TextMeshProUGUI flamethrowerText;
    private TextMeshProUGUI molotovText;

    private AudioSource audioSource;
    public AudioClip AmmoSound;
    public AudioClip maxAmmoSound;

    public int ammoAmount;
    public int maxAmmo;

    public bool shotgunKit;
    public bool rocketKit;
    public bool fuelCan;
    public bool molotoves;

    void Start(){
        shotgunAmmoText = GameObject.Find("ShotgunAmmo").GetComponent<TextMeshProUGUI>();
        rocketAmmoText = GameObject.Find("RocketLauncherAmmo").GetComponent<TextMeshProUGUI>();
        flamethrowerText = GameObject.Find("FlamethrowerAmmo").GetComponent<TextMeshProUGUI>();
        molotovText = GameObject.Find("MolotovAmount").GetComponent<TextMeshProUGUI>();

        audioSource = GetComponent<AudioSource>();
    }

    void OnTriggerEnter(Collider other){
        if(other.gameObject.tag == "PlayerModel" && shotgunKit){
            if(int.TryParse(shotgunAmmoText.text, out int currentAmmo)){
                int newAmmo = currentAmmo + ammoAmount;
                newAmmo = Mathf.Clamp(newAmmo, 0, maxAmmo);

                if(currentAmmo == maxAmmo){
                    audioSource.clip = maxAmmoSound;
                    audioSource.Play();
                    return;
                }

                if(currentAmmo <= maxAmmo){
                    shotgunAmmoText.text = newAmmo.ToString();
                    audioSource.clip = AmmoSound;
                    audioSource.Play();
                    Destroy(this.gameObject, 0.22f);
                    }
                }
            }

        if(other.gameObject.tag == "PlayerModel" && rocketKit){
            if(int.TryParse(rocketAmmoText.text, out int currentAmmo)){
                int newAmmo = currentAmmo + ammoAmount;
                newAmmo = Mathf.Clamp(newAmmo, 0, maxAmmo);

                if(currentAmmo == maxAmmo){
                    audioSource.clip = maxAmmoSound;
                    audioSource.Play();
                    return;
                }

                if(currentAmmo <= maxAmmo){
                    rocketAmmoText.text = newAmmo.ToString();
                    audioSource.clip = AmmoSound;
                    audioSource.Play();
                    Destroy(this.gameObject, 0.22f);
                }
            }
        }

        if(other.gameObject.tag == "PlayerModel" && fuelCan){
            if(int.TryParse(flamethrowerText.text, out int currentAmmo)){
                int newAmmo = currentAmmo + ammoAmount;
                newAmmo = Mathf.Clamp(newAmmo, 0, maxAmmo);

                if(currentAmmo == maxAmmo){
                    audioSource.clip = maxAmmoSound;
                    audioSource.Play();
                    return;
                }

                if(currentAmmo <= maxAmmo){
                    flamethrowerText.text = newAmmo.ToString();
                    audioSource.clip = AmmoSound;
                    audioSource.Play();
                    Destroy(this.gameObject, 0.22f);
                }
            }
        }

        if(other.gameObject.tag == "PlayerModel" && molotoves){
            if(int.TryParse(molotovText.text, out int currentAmmo)){
                int newAmmo = currentAmmo + ammoAmount;
                newAmmo = Mathf.Clamp(newAmmo, 0, maxAmmo);

                if(currentAmmo == maxAmmo){
                    audioSource.clip = maxAmmoSound;
                    audioSource.Play();
                    return;
                }

                if(currentAmmo <= maxAmmo){
                    molotovText.text = newAmmo.ToString();
                    audioSource.clip = AmmoSound;
                    audioSource.Play();
                    Destroy(this.gameObject, 0.22f);
                }
            }
        }
    }
}