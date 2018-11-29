using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public struct ShotInformation {
    public int damage;
    public string tag;
}

public class BaseCharacter : MonoBehaviour {

    protected Rigidbody body;
    protected Health health;
    public float movementSpeed = 5.0f;

    virtual
    public void Init() {
        body = GetComponent<Rigidbody>();
        health = GetComponent<Health>(); 
    }

    public Rigidbody GetRigidbody() {
        return body;
    }

    public Health GetHealth() {
        return health;
    }

    virtual
    protected void Shot(ShotInformation info) { }

    virtual
    public void Hit(int damage) {
        if (health.takeDamage(damage) <= 0) {
            Debug.Log("dead");
        }
    }
}
