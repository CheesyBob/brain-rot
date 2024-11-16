using UnityEngine;
using System.Collections;
using TMPro;

public class MolotovThrow : MonoBehaviour
{
    public Transform spawnPoint;
    public GameObject molotovPrefab;
    public GameObject MolotovBase;
    public AudioClip noAmmoSound;
    private TextMeshProUGUI ammoText;
    private int currentAmmo;
    private int removeModifier = 1;
    public float throwForce;
    private bool ableToShoot = true;
    public int molotovsToThrow;
    private Camera playerCamera;

    public void Start()
    {
        playerCamera = GameObject.Find("PlayerCamera").GetComponent<Camera>();
        ammoText = GameObject.Find("MolotovAmount").GetComponent<TextMeshProUGUI>();
    }

    public void Update()
    {
        if (Input.GetButtonDown("Fire1") && ableToShoot)
        {
            for (int i = 0; i < molotovsToThrow; i++)
            {
                if (currentAmmo > 0)
                {
                    Throw();
                }
                else
                {
                    break;
                }
            }
        }

        if (Input.GetMouseButtonDown(0) && !ableToShoot)
        {
            GetComponent<AudioSource>().PlayOneShot(noAmmoSound);
        }

        if (int.TryParse(ammoText.text, out currentAmmo))
        {
            UpdateText();
        }

        if(ammoText.text == "0"){
            MolotovBase.SetActive(false);
        }
        else{
            MolotovBase.SetActive(true);
        }

        AmmoCheck();
    }

    public void Throw()
    {
        GameObject newObject = Instantiate(molotovPrefab, spawnPoint.position, Quaternion.identity);
        Rigidbody rb = newObject.GetComponent<Rigidbody>();

        rb.isKinematic = false;
        Vector3 targetPoint = GetMouseTargetPoint();
        Vector3 throwDirection = (targetPoint - spawnPoint.position).normalized;
        rb.AddForce(throwDirection * throwForce, ForceMode.Impulse);

        RemoveAmmo();
    }

    private Vector3 GetMouseTargetPoint()
    {
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            return hit.point;
        }
        return ray.GetPoint(1000);
    }

    private void UpdateText()
    {
        ammoText.text = currentAmmo.ToString();
    }

    private void RemoveAmmo()
    {
        if (currentAmmo > 0)
        {
            currentAmmo -= removeModifier;
            UpdateText();
        }
    }

    private void AmmoCheck()
    {
        ableToShoot = currentAmmo > 0;

        if (ammoText.text == "-1")
        {
            ammoText.text = "0";
        }
    }
}