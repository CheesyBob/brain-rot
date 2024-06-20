using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponEquipedStatus : MonoBehaviour
{
    private GameObject Pistol;
    private GameObject Shotgun;
    private GameObject RocketLauncher;
    private GameObject flamethrower;

    public bool pistolEquiped;
    public bool shotgunEquiped;
    public bool rocketLauncherEquiped;
    public bool flamethrowerEquiped;

    void Start(){
        Pistol = DestroyClonedPlayer.Instance.playerPistol;
        Shotgun = DestroyClonedPlayer.Instance.playerShotgun;
        RocketLauncher = DestroyClonedPlayer.Instance.playerRocketLauncher;
        flamethrower = DestroyClonedPlayer.Instance.playerFlamethrower;
    }

    void Update(){
        if(Pistol.activeSelf){
            pistolEquiped = true;
            shotgunEquiped = false;
        }
        else{
            pistolEquiped = false;
        }

        if(Shotgun.activeSelf){
            shotgunEquiped = true;
            pistolEquiped = false;
        }
        else{
            shotgunEquiped = false;
        }

        if(RocketLauncher.activeSelf){
            rocketLauncherEquiped = true;
            pistolEquiped = false;
            shotgunEquiped = false;
        }
        else{
            rocketLauncherEquiped = false;
        }

        if(flamethrower.activeSelf){
            flamethrowerEquiped = true;
            rocketLauncherEquiped = false;
            pistolEquiped = false;
            shotgunEquiped = false;
        }
        else{
            flamethrowerEquiped = false;
        }
    }
}