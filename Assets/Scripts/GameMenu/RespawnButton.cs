using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

public class RespawnButton : MonoBehaviour
{
    private GameObject DeathPannel;
    private GameObject WeaponHolder;
    public GameObject player;
    public GameObject respawnPannel;

    public Animator playerAnimator;

    private TextMeshProUGUI healthText;

    private int previousSceneIndex;

    void Awake(){
        WeaponHolder = GameObject.Find("WeaponHolder");
    }

    void Start()
    {
        DeathPannel = GameObject.Find("DeathPannel");
        healthText = GameObject.Find("HealthText").GetComponent<TextMeshProUGUI>();
    }

    public void OnClick()
    {
        healthText.text = "100";

        respawnPannel.SetActive(false);

        player.GetComponent<HealthStatus>().death = false;
        player.GetComponent<SwitchItems>().enabled = true;
        player.GetComponent<PlayerMovement>().enabled = true;

        WeaponHolder.SetActive(true);

        playerAnimator.SetBool("playerDied", false);
        playerAnimator.SetBool("playerDied2", false);

        DeathPannel.GetComponent<DeathFadeIn>().isFading = false;

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}