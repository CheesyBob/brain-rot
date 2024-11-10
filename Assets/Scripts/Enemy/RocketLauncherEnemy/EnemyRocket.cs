using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyRocket : MonoBehaviour
{
    private GameObject PainPannel;

    private TextMeshProUGUI healthText;

    public AudioClip explosionSound;

    public float radius;
    public int damageAmount;

    void Start(){
        healthText = GameObject.Find("HealthText").GetComponent<TextMeshProUGUI>();
        PainPannel = GameObject.Find("PainPannel");
    }

    void OnTriggerEnter(Collider other){
        if(other.gameObject.tag == "Wall"){
            Explode();
        }
        
        if(other.gameObject.tag == "PlayerModel"){
            Explode();
        }

        if(other.gameObject.tag == "Barrel"){
            other.gameObject.GetComponent<BarrelExplode>().Explode();

            Explode();
        }
    }

    void Explode(){
        Collider[] playerColliders = Physics.OverlapSphere(transform.position, radius);

        foreach (Collider col in playerColliders){
            if(col.CompareTag("PlayerModel")){
                col.GetComponent<HealthStatus>().DamagePlayer(50);
            }
        }

        GetComponent<MeshRenderer>().enabled = false;

        GetComponent<CapsuleCollider>().enabled = false;

        GetComponent<Rigidbody>().isKinematic = true;

        GetComponent<AudioSource>().PlayOneShot(explosionSound);

        Transform firstChild = transform.GetChild(0);
        firstChild.gameObject.SetActive(true);

        Transform secondChild = transform.GetChild(1);
        secondChild.gameObject.GetComponent<ParticleSystem>().Stop();

        Destroy(gameObject, 2f);
    }
}