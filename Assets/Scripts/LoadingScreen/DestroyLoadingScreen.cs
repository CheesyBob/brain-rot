using UnityEngine;

public class DestroyLoadingScreen : MonoBehaviour
{
    public GameObject Background;
    public GameObject Text;

    public bool canShoot = false;
    public bool startLevel = false;

    public GameObject[] objectsToDisable;

    void Start()
    {
        foreach (GameObject obj in objectsToDisable)
        {
            obj.SetActive(false);
        }
    }

    void Update()
    {
        if (GameObject.Find("PlayerModel").GetComponent<PlayerMovement>().playerMoving)
        {
            canShoot = true;
            startLevel = true;

            gameObject.GetComponent<DestroyLoadingScreen>().enabled = false;

            Destroy(Background);
            Destroy(Text);

            GetComponent<AudioSource>().Pause();

            foreach (GameObject obj in objectsToDisable)
            {
                obj.SetActive(true);
            }
        }
    }
}