using UnityEngine;

public class DestroyClonedPlayer : MonoBehaviour
{
    public static DestroyClonedPlayer Instance { get; private set; }

    public GameObject playerCamera;
    public GameObject playerPistol;
    public GameObject playerShotgun;
    public GameObject playerRocketLauncher;
    public GameObject playerFlamethrower;

    private static bool playerExists;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (playerExists)
        {
            Destroy(gameObject);
        }
        else
        {
            playerExists = true;

            DontDestroyOnLoad(gameObject);
        }
    }
}