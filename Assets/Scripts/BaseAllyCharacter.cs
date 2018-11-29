using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseAllyCharacter : BaseCharacter {

    //Currently held gun info
    protected GameObject heldGun;
    protected GunController gunController;
    protected PlayerInventory playerInventory;

    override
    public void Init() {
        base.Init();
        playerInventory = GetComponent<PlayerInventory>();
        heldGun = playerInventory.GetHeldGun();
        gunController = heldGun.GetComponent<GunController>();
    }

    public GameObject GetHeldGun() {
        return heldGun;
    }
}
