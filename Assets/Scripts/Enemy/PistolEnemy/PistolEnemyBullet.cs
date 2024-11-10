using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PistolEnemyBullet : MonoBehaviour
{
    private GameObject PainPannel;

    private TextMeshProUGUI healthText;

    void Start(){
        healthText = GameObject.Find("HealthText").GetComponent<TextMeshProUGUI>();
        PainPannel = GameObject.Find("PainPannel");
    }

    void OnTriggerEnter(Collider other){
        if(other.CompareTag("PlayerModel")){
            other.GetComponent<HealthStatus>().DamagePlayer(3);

            Destroy(gameObject);
        }
        if(other.CompareTag("Wall")){
            Destroy(gameObject);
        }
    }
}