using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerRocket : MonoBehaviour
{
    private GameObject PainPannel;
    private TextMeshProUGUI healthText;

    public float radius;
    public int damageAmount;

    void Start(){
        healthText = GameObject.Find("HealthText").GetComponent<TextMeshProUGUI>();
        PainPannel = GameObject.Find("PainPannel");

        Destroy(gameObject, 6f);
    }

    void Update(){
        if (GetComponent<Rigidbody>().velocity.sqrMagnitude > 0f)
        {
            Vector3 horizontalVelocity = new Vector3(GetComponent<Rigidbody>().velocity.x, 0, GetComponent<Rigidbody>().velocity.z);

            if (horizontalVelocity.sqrMagnitude > 0f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(horizontalVelocity.normalized);

                targetRotation = Quaternion.Euler(90f, targetRotation.eulerAngles.y, targetRotation.eulerAngles.z);

                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 100f);
            }
        }
    }

    void OnTriggerEnter(Collider other){
        if(other.gameObject.tag == "Wall"){
            Explode();
        }

        if(other.gameObject.tag == "Enemy"){
            Explode();
        }

        if(other.gameObject.tag == "Sentry"){
            other.gameObject.GetComponent<SentryShoot>().Explode();
            Explode();
        }

        if(other.gameObject.tag == "Ground"){
            Explode();
        }

        if(other.gameObject.tag == "Barrel"){
            other.gameObject.GetComponent<BarrelExplode>().Explode();

            Explode();
        }
    }

    void Explode(){
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius, LayerMask.GetMask("Enemy", "Casual"));
        Collider[] playerColliders = Physics.OverlapSphere(transform.position, radius);

        foreach (Collider col in colliders){
            float distance = Vector3.Distance(transform.position, col.transform.position);

            if (distance <= radius){
                EnemyAI component = col.GetComponent<EnemyAI>();
                if (component != null) {
                    component.currentHealth -= 100f;
                }

                if(col.CompareTag("Casual")){
                    col.GetComponent<CasualAI>().currentHealth -= 100f;
                }
            }
        }

        foreach (Collider col in playerColliders){
            if(col.CompareTag("PlayerModel")){
                col.GetComponent<HealthStatus>().DamagePlayer(50);
            }
        }

        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<CapsuleCollider>().enabled = false;
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<AudioSource>().Play();

        Transform firstChild = transform.GetChild(0);
        firstChild.gameObject.SetActive(true);

        Transform secondChild = transform.GetChild(1);
        secondChild.gameObject.GetComponent<ParticleSystem>().Stop();

        Destroy(gameObject, 2f);
    }
}