using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RocketLauncherShoot : MonoBehaviour
{
    public GameObject rocket;

    public Transform spawnPoint;

    private TextMeshProUGUI ammoText;
    
    private Camera playerCamera;

    public AudioClip rocketFireSound;
    public AudioClip noAmmoSound;

    public float spawnForce;

    private int currentAmmo;
    private int removeModifier = 1;

    private bool ableToShoot = true;
    public bool shot = false;

    void Start(){
        ammoText = GameObject.Find("RocketLauncherAmmo").GetComponent<TextMeshProUGUI>();
        playerCamera = DestroyClonedPlayer.Instance.playerCamera.GetComponent<Camera>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && ableToShoot)
        {
            SpawnNewObject();
        }

        if(int.TryParse(ammoText.text, out currentAmmo))
        {
            UpdateText();
        }

        if(Input.GetMouseButtonDown(0) && !ableToShoot){
            GetComponent<AudioSource>().PlayOneShot(noAmmoSound);
        }

        AmmoCheck();
    }

    public void SpawnNewObject()
    {
        GameObject newObject = Instantiate(rocket, spawnPoint.position, Quaternion.identity);

        newObject.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
        newObject.GetComponent<AudioSource>().PlayOneShot(rocketFireSound);

        shot = true;

        RemoveAmmo();
        MoveTowardsMouse(newObject);
    }

    void MoveTowardsMouse(GameObject obj)
    {
        Rigidbody rb = obj.GetComponent<Rigidbody>();

        Vector3 mousePosition = GetMouseWorldPosition();
        Vector3 direction = mousePosition - obj.transform.position;

        direction.y = 0f;
        direction.Normalize();

        Quaternion targetRotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));

        rb.MoveRotation(targetRotation);

        rb.AddForce(direction * spawnForce, ForceMode.Impulse);
    }

    private Vector3 GetMouseWorldPosition()
    {
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(Vector3.up, transform.position);

        float distance;

        if (plane.Raycast(ray, out distance))
        {
            Vector3 hitPoint = ray.GetPoint(distance);
            return hitPoint;
        }

        return playerCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, distance));
    }

    private void UpdateText()
    {
        ammoText.text = currentAmmo.ToString();
    }

    private void RemoveAmmo(){
        if(currentAmmo >= 0)
        {
            currentAmmo -= removeModifier;
            
            UpdateText();
        }
    }

    private void AmmoCheck(){
        if(currentAmmo >= 0){
            ableToShoot = true;
        }

        if(currentAmmo <= 0){
            ableToShoot = false;
        }

        if(ammoText.text == "-1"){
            ammoText.text = "0";
        }
    }
}