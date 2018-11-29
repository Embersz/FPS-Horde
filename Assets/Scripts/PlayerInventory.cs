using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour {
    public GameObject[] guns;
    private int currentWeapon = 0;

    private void Start() {
        guns[0].SetActive(true);
        guns[0].GetComponent<GunController>().SetHeld(true, transform);
        for (int i = 1; i < guns.Length; i++) {
            if (guns[i] != null) {
                guns[i].SetActive(false);
                guns[i].GetComponent<GunController>().SetHeld(true, transform);
            }
        }
    }

    //Drops gun and switches to the previous weapon should it exist
    public GameObject DropGun(bool reorganizeGuns) {
        if(guns[currentWeapon].GetComponent<GunController>().canDrop) {
            guns[currentWeapon].GetComponent<GunController>().SetHeld(false, transform);
            guns[currentWeapon] = null;

            if(reorganizeGuns) {
                //Move all guns down one to fill hole left from dropping
                for (int i = currentWeapon; i < guns.Length - 1; i++) {
                    guns[i] = guns[i + 1];
                    guns[i + 1] = null;
                }

                if (!guns[currentWeapon])
                    currentWeapon--;
            }



        }
        return guns[currentWeapon];
    }

    public void PickUpGun(GameObject gun) {

        //Already held by another NPC/player so we early quit
        if (gun.GetComponent<GunController>().IsHeld())
            return;

        bool emptySpot = false;
        for (int i = 0; i < guns.Length; i++) {
            if (guns[i] == null) {
                gun.GetComponent<GunController>().SetHeld(true, transform);

                //TODO: This is hacky and needs to be reworked
                gun.transform.localRotation = new Quaternion(0, 0, 0, 0);
                gun.transform.localPosition = new Vector3(.35f, -.35f, .6f);
                gun.transform.localScale = new Vector3(1, 1, 1);

                gun.SetActive(false);
                guns[i] = gun;
                emptySpot = true;
                break;
            }
        }
        if (!emptySpot) {
            if (guns[currentWeapon].GetComponent<GunController>().canDrop) {
                DropGun(false);
                gun.GetComponent<GunController>().SetHeld(true, transform);
                guns[currentWeapon] = gun;
               
                //TODO: This is hacky and needs to be reworked
                gun.transform.localRotation = new Quaternion(0, 0, 0, 0);
                gun.transform.localPosition = new Vector3(.35f, -.35f, .6f);
                gun.transform.localScale = new Vector3(1, 1, 1);
            }
        }
    }

    public GameObject GetHeldGun() {
        return guns[currentWeapon];
    }

    public GameObject SwapGun() {
        currentWeapon = (currentWeapon + 1) % guns.Length;
        if (guns[currentWeapon] == null) {
            currentWeapon = 0;
        }

        return guns[currentWeapon];
    }
}
