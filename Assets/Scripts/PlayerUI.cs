using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour {

    public GameObject player;

    public Text uiHealth;
    public Text uiArmor;

    //Remaining ammo not loaded
    public Text uiRemainingAmmo;
    //Currently loaded
    public Text uiClipAmmo;

    private Health health;

    private GameObject heldGun;
    private GunController gunInfo;

    void Start () {
        health = player.GetComponent<Health>();
        heldGun = player.GetComponent<PlayerController>().GetHeldGun();

        uiHealth.text = health.currentHealth.ToString();
        uiArmor.text = health.currentArmor.ToString();
    }
	
	
	void Update () {
        heldGun = player.GetComponent<PlayerController>().GetHeldGun();

        if(heldGun)
            gunInfo = heldGun.GetComponent<GunController>();

        UpdateUI();
    }

    private void UpdateUI() {
        uiHealth.text = health.currentHealth.ToString();
        uiArmor.text = health.currentArmor.ToString();

        if(heldGun) {
            uiClipAmmo.text = gunInfo.GetAmmoInClip().ToString();
            uiRemainingAmmo.text = gunInfo.GetAmmoNotInClip().ToString();
        } else {
            uiClipAmmo.text = "0";
            uiRemainingAmmo.text = "0";
        }

    }
}
