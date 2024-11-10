using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DeathwadFireAmount : MonoBehaviour
{
    public TextMeshProUGUI rocketAmmoText;
    public TextMeshProUGUI molotovAmmoText;
    public TextMeshProUGUI deathwadAmmoText;

    private int removeModifier = 1;

    public void Update(){
        UpdateDeathwadAmmo();
    }

    public void DecreaseAmmo(TextMeshProUGUI ammoText)
    {
        int ammo;
        if (int.TryParse(ammoText.text, out ammo) && ammo > 0)
        {
            ammo -= removeModifier;
            ammoText.text = ammo.ToString();
        }
    }

    public void UpdateDeathwadAmmo()
    {
        int rocketAmmo, molotovAmmo;
        if (int.TryParse(rocketAmmoText.text, out rocketAmmo) && int.TryParse(molotovAmmoText.text, out molotovAmmo))
        {
            int fireableTimes = (rocketAmmo > 0 && molotovAmmo > 0) ? Mathf.Min(rocketAmmo, molotovAmmo) : 0;
            deathwadAmmoText.text = fireableTimes.ToString();
        }
    }

    public bool CanShoot()
    {
        return HasAmmo(rocketAmmoText) && HasAmmo(molotovAmmoText);
    }

    private bool HasAmmo(TextMeshProUGUI ammoText)
    {
        int ammo;
        return int.TryParse(ammoText.text, out ammo) && ammo > 0;
    }
}