using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LaserGunController : MonoBehaviour {

    private Camera mainCamera;
    private AudioSource gunNoise;
    private AudioSource reloadNoise;

    private float shotTime;
    public float shootDelay;

    public int maxReserve;
    public int maxClip;

    protected int clip = 30;
    protected int reserve = 90;

    public int damage = 50;

    private float reloadStart = 0.0f;
    //1 second reload time by default
    public float reloadTime = 1.0f;
    private bool reloading = false;


    // Use this for initialization
	void Start ()
    {
        mainCamera = Camera.main;
        var audio = GetComponents<AudioSource>();
        gunNoise    = audio[0];
        reloadNoise = audio[1];
	}
	
	// Update is called once per frame
	void Update () {
        if (reloading) {
            if (Time.time - reloadStart <= reloadTime) {
                reloading = false;
            }

        }
    }

    //TODO:Reloading isn't delaying like I want it too can fix this later
    public void reload() {

        int neededShots = maxClip - clip;

        if(reserve >= neededShots) {
            clip += neededShots;
            reserve -= neededShots;
        } else{
            if (reserve > 0) {
                clip += reserve;
                reserve = 0;
            } else {
                return;
            }
        }

        reloading = true;
        reloadStart = Time.time;
    }

    public void addAmmo()
    {
        reserve += maxReserve;
    }

    public int getAmmoInClip() {
        return clip;
    }

    public int getAmmoNotInClip() {
        return reserve;
    }
    public void fireBullet() {
        if(clip > 0 && !reloading) {
            if (Time.time - shotTime >= shootDelay) {
                shotTime = Time.time;

                Vector3 cameraDir;
                Vector3 cameraPos;
                if(transform.root.tag == "Player")
                {
                    cameraDir = mainCamera.transform.forward;
                    cameraPos = mainCamera.transform.position;
                }
                else
                {
                    cameraDir = transform.forward;
                    cameraPos = transform.position;
                }
                RaycastHit results;

                gunNoise.Play();
                if (Physics.Raycast(cameraPos, cameraDir, out results)) {
                    if (results.collider.tag == "WeakPoint")
                        results.rigidbody.SendMessage("CriticalHit", damage);

                    else if (results.collider.tag == "Enemy")
                        results.collider.SendMessage("Shot", damage);


                }
                clip--;
            }
        } else {
            reloadNoise.Play();
            reload();
        }
    }
}
