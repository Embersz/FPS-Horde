using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FireType {
    Automatic,
    Semi,
    Burst
}

public class GunController : MonoBehaviour {

    public bool canDrop = true;

    private bool held = false;
    private bool triggerHeld = false;

    public FireType gunFireType;
    private Camera mainCamera;
    private AudioSource gunNoise;
    private AudioSource reloadNoise;

    //Shooting information
    public int damage = 50;
    public float shootDelay;
    public float burstDelay;
    public int numOfBurstShots;
    private int burstCount;
    private float shotTime;

    //Ammo information
    public int maxAmmo = 90;
    public int maxLoadedAmmo = 30;
    public int loadedAmmo = 30;
    public int unloadedAmmo = 90;
    public bool infiniteAmmo = false;

    //Reload information
    public float reloadTime = 1.0f;
    private bool reloading = false;

    void Start() {
        mainCamera = Camera.main;
        var audio = GetComponents<AudioSource>();
        gunNoise = audio[0];
        reloadNoise = audio[1];
        burstCount = 0;
    }

    void Update() {
        
    }

    public bool IsHeld() {
        return held;
    }

    public void Reload() {
        if (!reloading) {
            if (unloadedAmmo > 0) {
                reloading = true;
                reloadNoise.Play();
                Invoke("DoReload", reloadTime);
            }
        }
    }
    private void DoReload() {
        int neededShots = maxLoadedAmmo - loadedAmmo;

        if (unloadedAmmo >= neededShots) {
            loadedAmmo += neededShots;
            unloadedAmmo -= neededShots;
        } else {
            if (unloadedAmmo > 0) {
                loadedAmmo += unloadedAmmo;
                unloadedAmmo = 0;
            }
        }

        reloading = false;
    }

    public void AddAmmo(int amount) {
        if(unloadedAmmo < maxAmmo) {
            if (unloadedAmmo + amount > maxAmmo)
                unloadedAmmo = maxAmmo;
            else
                unloadedAmmo += amount;
        }
    }

    public int GetAmmoInClip() {
        return loadedAmmo;
    }

    public int GetAmmoNotInClip() {
        return unloadedAmmo;
    }

    //TODO: Really don't like having to pass the parent
    public void SetHeld(bool flag, Transform parent) {
        if (canDrop) {

            held = flag;
            GetComponent<Rigidbody>().isKinematic = flag;
            GetComponent<BoxCollider>().isTrigger = flag;

            if (flag) {
                if (parent.gameObject.tag == "Player")
                    transform.parent = Camera.main.transform;
            } else {
                transform.parent = null;
            }
        }
    }

    private void FireBullet() {
        if (++burstCount == numOfBurstShots) CancelInvoke("FireBullet");

        Vector3 cameraDir;
        Vector3 cameraPos;

        //TODO: I don't think we should have to do these checks. Maybe we should pass to the guncontroller where the bullet should exit
        if (transform.root.tag == "Player") {
            cameraDir = mainCamera.transform.forward;
            cameraPos = mainCamera.transform.position;
        } else {
            cameraDir = transform.forward;
            cameraPos = transform.position;
            if (transform.root.tag == "Ally")
                transform.root.SendMessage("ShootAnimation");
        }

        RaycastHit results;
        if (Physics.Raycast(cameraPos, cameraDir, out results)) {
            ShotInformation info = new ShotInformation();
            info.damage = damage;
            info.tag = results.collider.tag;

            //We send the shot to the root of the collider we shot. This might not be ideal if we want gun shots to appear where the "bullet" hits
            results.collider.transform.root.SendMessage("Shot", info, SendMessageOptions.DontRequireReceiver);
        }

        gunNoise.Play();

        if (!infiniteAmmo) {
            if (--loadedAmmo == 0) {
                CancelInvoke("FireBullet");
                if(!reloading) {
                    Reload();
                }
            }
        }
    }

    //Returns true if we actually shot or not.
    public bool Shoot() {
        if (loadedAmmo > 0 && !reloading) {
            if (Time.time - shotTime >= shootDelay) {
                shotTime = Time.time;

                switch (gunFireType) {
                    case FireType.Automatic:
                        FireBullet();
                        break;
                    case FireType.Burst:
                        burstCount = 0;
                        InvokeRepeating("FireBullet", 0, burstDelay);
                        break;
                    case FireType.Semi:
                        if (triggerHeld)
                            return false;
                        else
                            FireBullet();
                        break;
                }
            }
            triggerHeld = true;
            return true;

        } else {
            return false;
        }
    }

    public void SetShooting(bool flag) {
        triggerHeld = flag;
    }
}

