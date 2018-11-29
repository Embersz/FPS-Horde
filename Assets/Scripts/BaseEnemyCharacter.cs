using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemyCharacter : BaseCharacter {

    public int damage;
    public int attackSpeed;
    protected float attackTime;

    protected GameObject player;
    protected GameObject target;
    protected GameObject[] targets;
     
    override
    public void Init() {
        base.Init();
        player = GameObject.FindGameObjectWithTag("Player"); //find a player
        targets = GameObject.FindGameObjectsWithTag("Ally"); //make an array of all ally NPC's
    }
    protected void Attack() {
        attackTime = Time.time;
        target.SendMessage("Hit", damage);
    }

    override
    protected void Shot(ShotInformation info) {
        health.takeDamage(damage);
    }

    override
    public void Hit(int damage)
    {
        health.takeDamage(damage);
    }
}
