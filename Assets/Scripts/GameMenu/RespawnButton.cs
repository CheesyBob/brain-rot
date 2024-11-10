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
    public TextMeshProUGUI shotgunText;
    public TextMeshProUGUI rocketLauncherText;
    public TextMeshProUGUI flamethrowerText;
    public TextMeshProUGUI molotovText;

    private int previousSceneIndex;

    public string initialShotgunText;
    public string initialRocketLauncherText;
    public string initialFlamethrowerText;
    public string initialMolotovText;

    void Awake(){
        WeaponHolder = GameObject.Find("WeaponHolder");

        initialShotgunText = shotgunText.text;
        initialRocketLauncherText = rocketLauncherText.text;
        initialFlamethrowerText = flamethrowerText.text;
        initialMolotovText = molotovText.text;
    }

    void Start()
    {
        DeathPannel = GameObject.Find("DeathPannel");
        healthText = GameObject.Find("HealthText").GetComponent<TextMeshProUGUI>();
    }

    public void OnClick()
    {
        healthText.text = "100";
        shotgunText.text = initialShotgunText;
        rocketLauncherText.text = initialRocketLauncherText;
        flamethrowerText.text = initialFlamethrowerText;
        molotovText.text = initialMolotovText;

        respawnPannel.SetActive(false);

        player.GetComponent<HealthStatus>().dead = false;
        player.GetComponent<SwitchItems>().enabled = true;
        player.GetComponent<PlayerMovement>().enabled = true;

        WeaponHolder.SetActive(true);

        playerAnimator.SetBool("playerDied", false);

        DeathPannel.GetComponent<DeathFadeIn>().isFading = false;

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}