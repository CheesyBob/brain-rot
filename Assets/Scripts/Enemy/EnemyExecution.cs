using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyExecution : MonoBehaviour
{
    public GameObject EnemyMain;

    private TextMeshProUGUI ShotgunAmmo;

    public AudioClip executionSound;
    public AudioClip rocketLauncherSound;
    public AudioClip flamethrowerSound;

    public KeyCode ExecutionKey;

    public float radius;
    public float executionTime;

    private bool enablePlayer = false;
    void Start()
    {
        ShotgunAmmo = GameObject.Find("ShotgunAmmo").GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        Collider[] playerColliders = Physics.OverlapSphere(transform.position, radius);

        foreach (Collider col in playerColliders)
        {
            if (col.CompareTag("PlayerModel"))
            {
                if (Input.GetKeyDown(ExecutionKey) && !col.GetComponent<HealthStatus>().death)
                {
                    if (!enablePlayer)
                    {
                        if (col.GetComponent<WeaponEquipedStatus>().pistolEquiped)
                        {
                            executionTime = 1.25f;

                            col.GetComponent<PlayerMovement>().enableMovement = false;
                            col.GetComponent<AudioSource>().PlayOneShot(executionSound);

                            GameObject.Find("Player").GetComponent<Animator>().SetBool("playerExecute", true);

                            StartCoroutine("EnablePlayer");
                            StartCoroutine("WaitToShootPistol");
                        }

                        if (col.GetComponent<WeaponEquipedStatus>().shotgunEquiped && ShotgunAmmo.text != "0")
                        {
                            executionTime = 1.2f;

                            col.GetComponent<PlayerMovement>().enableMovement = false;
                            col.GetComponent<AudioSource>().PlayOneShot(executionSound);

                            GameObject.Find("Player").GetComponent<Animator>().SetBool("playerExecute", true);

                            StartCoroutine("EnablePlayer");
                            StartCoroutine("WaitToShootShotgun");
                        }

                        if (col.GetComponent<WeaponEquipedStatus>().rocketLauncherEquiped)
                        {
                            executionTime = 1.8f;

                            col.GetComponent<PlayerMovement>().enableMovement = false;
                            col.GetComponent<AudioSource>().PlayOneShot(executionSound);

                            GameObject.Find("Player").GetComponent<Animator>().SetBool("playerExecute", true);

                            StartCoroutine("WaitToPlayRocketLauncherSound");
                            StartCoroutine("EnablePlayer");
                        }

                        if (col.GetComponent<WeaponEquipedStatus>().flamethrowerEquiped)
                        {
                            executionTime = 2.3f;

                            col.GetComponent<PlayerMovement>().enableMovement = false;
                            col.GetComponent<AudioSource>().PlayOneShot(executionSound);

                            GameObject.Find("Player").GetComponent<Animator>().SetBool("playerExecute", true);

                            StartCoroutine("EnableFlames");
                            StartCoroutine("EnablePlayer");
                        }
                    }
                }
                if (enablePlayer)
                {
                    col.GetComponent<PlayerMovement>().enableMovement = true;
                }
            }
        }
    }

    IEnumerator EnablePlayer()
    {
        yield return new WaitForSeconds(executionTime);
        enablePlayer = true;

        GameObject.Find("Player").GetComponent<Animator>().SetBool("playerExecute", false);

        EnemyMain.GetComponent<EnemyDeath>().dead = true;
    }

    IEnumerator WaitToShootPistol()
    {
        yield return new WaitForSeconds(.3f);
        GameObject.Find("Pistol").GetComponent<PistolShoot>().executeShoot = true;
        GameObject.Find("Pistol").GetComponent<PistolShoot>().Shoot();
        GameObject.Find("Pistol").GetComponent<PistolShoot>().executeShoot = false;
    }

    IEnumerator WaitToShootShotgun()
    {
        yield return new WaitForSeconds(.5f);
        GameObject.Find("Shotgun").GetComponent<ShotgunShoot>().executeShoot = true;
        GameObject.Find("Shotgun").GetComponent<ShotgunShoot>().Shoot();
        GameObject.Find("Shotgun").GetComponent<ShotgunShoot>().executeShoot = false;
    }

    IEnumerator WaitToPlayRocketLauncherSound()
    {
        yield return new WaitForSeconds(1.24f);
        GetComponent<AudioSource>().PlayOneShot(rocketLauncherSound);
    }

    IEnumerator EnableFlames()
    {
        yield return new WaitForSeconds(1f);

        GetComponent<AudioSource>().PlayOneShot(flamethrowerSound);

        GameObject.Find("FlameThrowerParticles").GetComponent<ParticleSystem>().Play();

        yield return new WaitForSeconds(.5f);

        GameObject.Find("FlameThrowerParticles").GetComponent<ParticleSystem>().Stop();
    }
}