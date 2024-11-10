using UnityEngine;
using System.Collections;

public class MolotovExplode : MonoBehaviour
{
    public GameObject Fire;
    public ParticleSystem FireTrail;

    public void OnCollisionEnter(Collision collision){
        Fire.SetActive(true);
        Fire.transform.SetParent(null);

        transform.localScale = new Vector3(0,0,0);

        GetComponent<AudioSource>().Play();
        Destroy(GetComponent<CapsuleCollider>());

        FireTrail.Stop();

        StartCoroutine("WaitToDestroy");
    }

    IEnumerator WaitToDestroy(){
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }
}