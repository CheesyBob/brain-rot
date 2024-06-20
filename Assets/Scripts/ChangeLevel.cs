using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ChangeLevel : MonoBehaviour
{
    public string level;

    private static bool sceneChanged = false;
    private bool hasPlayedSound = false;

    public void Update(){
        if(sceneChanged){
            StartCoroutine("Cooldown");
        }
        if(GetComponent<MeshRenderer>().enabled == true && !hasPlayedSound){
            GetComponent<AudioSource>().Play();

            hasPlayedSound = true;
        }
    }

    public void OnTriggerEnter(Collider other){
        if(!sceneChanged && other.CompareTag("PlayerModel")){
            SceneManager.LoadScene(level);

            sceneChanged = true;
        }
    }

    IEnumerator Cooldown(){
        yield return new WaitForSeconds(1f);
        sceneChanged = false;
    }
}