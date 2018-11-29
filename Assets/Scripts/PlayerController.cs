using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : BaseAllyCharacter {
    private Vector3 keyboardInputs = Vector3.zero;
    private Vector3 mouseInput = Vector3.zero;

    private Quaternion localRotation = Quaternion.identity;

    //TODO: Player rotation seems sketchy still. Might want to look into cleaning it up.
    private float rotationX = 0f;

    // Use this for initialization
    void Start () {
        base.Init();
    }
	
    // Update is called once per frame
    void Update () {
        HandleInput();
    }

    void FixedUpdate() {
        body.MoveRotation(localRotation);
        body.MovePosition(body.position + keyboardInputs * movementSpeed * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter(Collider other) {
        if(other.tag=="HealthPack") {
            HealthPack pack = other.GetComponent<HealthPack>();
            health.heal(pack.getHealAmount());
        }

        if (other.tag == "ArmorPack") {
            HealthPack pack = other.GetComponent<HealthPack>();
            health.healArmor(pack.getArmorAmount());
        }

        if (other.tag == "AmmoPack")
        {
            HealthPack pack = other.GetComponent<HealthPack>();
            gunController.AddAmmo(pack.getAmmoAmount());
        }
    }

    private void HandleInput() {
        if (Input.GetKeyUp(KeyCode.Escape)) {
            if (Cursor.lockState != CursorLockMode.Locked)
                Cursor.lockState = CursorLockMode.Locked;
            else
                Cursor.lockState = CursorLockMode.None;
        }

        if(Input.GetKeyDown(KeyCode.Tab)) {

            //This should swap out everything but it's not
            heldGun.SetActive(false);

            heldGun = playerInventory.SwapGun();
            heldGun.SetActive(true);

            gunController = heldGun.GetComponent<GunController>();
        }
        if(Input.GetKey(KeyCode.Mouse0)) {
            if (gunController)
                gunController.Shoot();
        }

        if(Input.GetKeyUp(KeyCode.Mouse0)) {
            if(gunController)
                gunController.SetShooting(false);
        }
        
        if(Input.GetKeyDown(KeyCode.R)) {
            gunController.Reload();
        }

        if(Input.GetKeyDown(KeyCode.E)) {
            Camera temp = Camera.main;
            RaycastHit results;
            if(Physics.Raycast(temp.transform.position, temp.transform.forward, out results)) {
                if (results.collider.gameObject.tag == "Gun") {
                    playerInventory.PickUpGun(results.collider.gameObject);
                    heldGun = playerInventory.GetHeldGun();
                    gunController = heldGun.GetComponent<GunController>();
                } 
            }
        }

        if(Input.GetKeyDown(KeyCode.G)) {
            heldGun = playerInventory.DropGun(true);
            heldGun.SetActive(true);
            if (heldGun)
                gunController = heldGun.GetComponent<GunController>();
            else
                gunController = null;
        }

        keyboardInputs = Vector3.zero;
        keyboardInputs.x = Input.GetAxis("Horizontal");
        keyboardInputs.z = Input.GetAxis("Vertical");

        keyboardInputs = transform.TransformDirection(keyboardInputs);

        mouseInput = Vector3.zero;
        mouseInput.x = Input.GetAxis("Mouse X");
        rotationX += mouseInput.x * 100f * Time.fixedDeltaTime;

        localRotation = Quaternion.Euler(0, rotationX, 0);
    }

    override
    public void Hit(int damage) {
        if (health.takeDamage(damage) <= 0) {
            Scene current = SceneManager.GetActiveScene();
            SceneManager.LoadScene(current.name);
        }    
    }

}

