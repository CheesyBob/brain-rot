using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SentryBullet : MonoBehaviour
{
    private GameObject Player;
    private GameObject PainPannel;

    private TextMeshProUGUI healthText;

    void Start(){
        Player = GameObject.Find("PlayerModel");
        healthText = GameObject.Find("HealthText").GetComponent<TextMeshProUGUI>();
        PainPannel = GameObject.Find("PainPannel");

        StartCoroutine(WaitToDestroy());
    }

    void OnTriggerEnter(Collider other){
        if(other.tag == "PlayerModel"){
            other.GetComponent<HealthStatus>().DamagePlayer(30);

            Destroy(gameObject);
        }
        if(other.tag == "Wall"){
            Destroy(gameObject);
        }
    }

    IEnumerator WaitToDestroy(){
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
}