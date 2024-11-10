using UnityEngine;
using UnityEngine.AI;

public class DestroyLoadingScreen : MonoBehaviour
{
    public GameObject Background;
    public GameObject Text;

    public bool canShoot = false;
    public bool startLevel = false;

    void Start(){
        NavMeshAgent[] agents = FindObjectsOfType<NavMeshAgent>();
        foreach (NavMeshAgent agent in agents)
        {
            agent.isStopped = true;
        }
    }

    void Update()
    {
        if (GameObject.Find("PlayerModel").GetComponent<PlayerMovement>().playerMoving)
        {
            canShoot = true;
            startLevel = true;

            Destroy(Background);
            Destroy(Text);

            GetComponent<AudioSource>().Pause();

            NavMeshAgent[] agents = FindObjectsOfType<NavMeshAgent>();
            foreach (NavMeshAgent agent in agents)
            {
                agent.isStopped = false;
            }

            gameObject.GetComponent<DestroyLoadingScreen>().enabled = false;
        }
    }
}