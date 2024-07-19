using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Camera mainCamera;

    private CharacterController characterController;

    private Vector3 lastMousePosition;

    private Animator playerAnimator;

    public float moveSpeed;
    public float rotationSpeed;
    private float maxRotationDistance;
    private float mouseDeadZone;
    public float gravity = 9.81f;

    public bool enableMovement = true;
    public bool playerMoving = false;

    void Start()
    {
        playerAnimator = GameObject.Find("Player").GetComponent<Animator>();

        characterController = GetComponent<CharacterController>();
        mainCamera = Camera.main;
    }

    void Update()
    {
        if(enableMovement){
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = Vector3.Distance(mainCamera.transform.position, transform.position);
            Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(mousePosition);

            if (Vector3.Distance(transform.position, mouseWorldPosition) > maxRotationDistance)
            {
                Vector3 directionToMouse = mouseWorldPosition - transform.position;
                directionToMouse.y = 0f;
                Quaternion rotationToMouse = Quaternion.LookRotation(directionToMouse);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotationToMouse, rotationSpeed * Time.deltaTime);
            }

            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");
            
            Vector3 movement = new Vector3(horizontalInput, 0f, verticalInput).normalized;

            if(movement.magnitude > mouseDeadZone)
            {
                characterController.Move(movement * moveSpeed * Time.deltaTime); 

                playerMoving = true;

                playerAnimator.SetBool("isRunning", true);

                //Checking if the pistol is equpied then play pistolrun

                if(this.gameObject.GetComponent<WeaponEquipedStatus>().pistolEquiped){
                    playerAnimator.SetBool("isPistolRunning", true);
                }

                else{
                    playerAnimator.SetBool("isRunning", true);
                }

                //Checking if the shotgun is equpied then play shotgunrun

                if(this.gameObject.GetComponent<WeaponEquipedStatus>().shotgunEquiped){
                    playerAnimator.SetBool("isShotgunRunning", true);
                }

                else{
                    playerAnimator.SetBool("isRunning", true);
                }

                //Checking if the rocketlauncher is equpied then play rocketlauncherrun

                if(this.gameObject.GetComponent<WeaponEquipedStatus>().rocketLauncherEquiped){
                    playerAnimator.SetBool("isRocketLauncherRunning", true);
                }

                else{
                    playerAnimator.SetBool("isRunning", true);
                }

                
                //Checking if the flamethrower is equpied then play flamethrowerun

                if(this.gameObject.GetComponent<WeaponEquipedStatus>().flamethrowerEquiped){
                    playerAnimator.SetBool("isFlamethroweruning", true);
                }

                else{
                    playerAnimator.SetBool("isRunning", true);
                }
            }

            //Stopping all of the players running animations when idle

            else{
                playerMoving = false;
                
                playerAnimator.SetBool("isRunning", false);
                playerAnimator.SetBool("isPistolRunning", false);
                playerAnimator.SetBool("isShotgunRunning", false);
                playerAnimator.SetBool("isRocketLauncherRunning", false);
                playerAnimator.SetBool("isFlamethroweruning", false);
            }

            //Checking if the pistol is equpied then play pistolidle

            if(this.gameObject.GetComponent<WeaponEquipedStatus>().pistolEquiped){
                playerAnimator.SetBool("pistolEquiped", true);
                playerAnimator.SetBool("isRunning", false);
            }

            else{
                playerAnimator.SetBool("pistolEquiped", false);
                playerAnimator.SetBool("isPistolRunning", false);
            }

            //Checking if the shotgun is equpied then play shotgunidle

            if(this.gameObject.GetComponent<WeaponEquipedStatus>().shotgunEquiped){
                playerAnimator.SetBool("shotgunEquiped", true);
                playerAnimator.SetBool("isRunning", false);
            }

            else{
                playerAnimator.SetBool("shotgunEquiped", false);
                playerAnimator.SetBool("isShotgunRunning", false);
            }

            //Checking if the rocketlauncher is equpied then play rocketlauncheridle

            if(this.gameObject.GetComponent<WeaponEquipedStatus>().rocketLauncherEquiped){
                playerAnimator.SetBool("rocketLauncherEquiped", true);
                playerAnimator.SetBool("isRunning", false);
            }

            else{
                playerAnimator.SetBool("rocketLauncherEquiped", false);
                playerAnimator.SetBool("isRocketLauncherRunning", false);
            }

            //Checking if the flamethrower is equpied then play flamethroweridle

            if(this.gameObject.GetComponent<WeaponEquipedStatus>().flamethrowerEquiped){
                playerAnimator.SetBool("flamethrowerEquiped", true);
                playerAnimator.SetBool("isRunning", false);
            }

            else{
                playerAnimator.SetBool("flamethrowerEquiped", false);
                playerAnimator.SetBool("isFlamethroweruning", false);
            }

            ApplyGravity();
        }
    }

    void ApplyGravity()
    {
        if (!characterController.isGrounded)
        {
            Vector3 gravityVector = Vector3.down * gravity;
            characterController.Move(gravityVector * Time.deltaTime);
        }
    }
}