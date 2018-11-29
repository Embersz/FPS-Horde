using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehavior : BaseEnemyCharacter
{
    private NavMeshAgent nav;
    private Animator anim;
    private bool panicked;

    public GameObject[] loot;

    public int minSpeed = 3;
    public int maxSpeed = 5;
    public float threatRadius = 10f;

	// Use this for initialization
	void Start ()
    {
        base.Init();
        nav = GetComponent<NavMeshAgent>(); //get NavMesh component.
        anim = GetComponent<Animator>();

        panicked = false;
        nav.speed = Random.Range(minSpeed, maxSpeed);
        InvokeRepeating("TargetClosestEnemy", 0, 0.25f);
        target = player;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (health.currentHealth <= 0)
            anim.SetTrigger("isDead");

        anim.SetFloat("Speed", nav.velocity.magnitude);

        if(target != null)
        {
            if (Vector3.Distance(transform.position, target.transform.position) > nav.stoppingDistance)
                nav.SetDestination(target.transform.position); //move to target's position.
            else
            {
                if (Time.time >= attackTime + attackSpeed)
                    anim.SetTrigger("attack");
            }


            //set rotation to face target.
            var targetRotation = Quaternion.LookRotation(target.transform.position - transform.position, Vector3.up);
            targetRotation.y = 180;
            //transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 1f);
        }

    }

    override
    protected void Shot(ShotInformation info)
    {

        if(info.tag == "WeakPoint") {
            health.takeDamage(info.damage*2);
        } else {
            health.takeDamage(info.damage);
        }
        
        //if currentHealth is below panic threshold.
        if (health.currentHealth <= health.maxHealth /5)
        {
            if(Random.Range(1, 10) == 1)
            {
                panicked = true;
                target = null;
            }
            InvokeRepeating("Decay", 0f, 0.5f);
        }
    }

    //Used to kill a panicking enemy.
    private void Decay()
    {
        health.takeDamage(1);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "HealthPack") {
            HealthPack pack = other.GetComponent<HealthPack>();
            health.heal(pack.getHealAmount());
        }

        if(other.tag == "ArmorPack") {
            HealthPack pack = other.GetComponent<HealthPack>();
            health.healArmor(pack.getArmorAmount());
        }
    }

    private void Destroy()
    {
        dropLoot();
        Destroy(gameObject);
    }

    private void dropLoot()
    {
        int i = Random.Range(1, 50);
        Vector3 dropPosition = transform.position;
        dropPosition.y = transform.position.y + 0.5f;
        Quaternion dropRotation = transform.rotation;

        switch (i)
        {
            case 1:
            case 2:
            case 3:
            case 4:
                Instantiate(loot[0], dropPosition, dropRotation);
                break;
            case 5:
                Instantiate(loot[0], dropPosition, dropRotation);
                dropLoot();
                break;
            case 6:
            case 7:
                Instantiate(loot[1], dropPosition, dropRotation);
                break;
            case 8:
                Instantiate(loot[1], dropPosition, dropRotation);
                dropLoot();
                break;
            case 9:
                Instantiate(loot[2], dropPosition, dropRotation);
                break;
            case 10:
                Instantiate(loot[2], dropPosition, dropRotation);
                dropLoot();
                break;
            default:
                break;
        }
    }

    private void TargetClosestEnemy()
    {
        target = null;
        //get a list of all colliders in radius
        Collider[] objectsInRange = Physics.OverlapSphere(transform.position, threatRadius);
        Vector3 position = transform.position; //position of invoking obj.
        GameObject closest = null;
        float distance = Mathf.Infinity;
        foreach (Collider col in objectsInRange)
        {
            if (!panicked) //if not panicked, attack player or allies.
            {
                if (col.tag == "Player" || col.tag == "Ally")
                {
                    Vector3 diff = col.transform.position - position;
                    float curDistance = diff.sqrMagnitude;
                    if (curDistance < distance)
                    {
                        closest = col.gameObject;
                        distance = curDistance;
                    }
                }
            }
            else //else if panicked, target anything.
            {
                if (col.tag == "Enemy" || col.tag == "Player"|| col.tag == "Ally")
                {
                    Vector3 diff = col.transform.position - position;
                    float curDistance = diff.sqrMagnitude;
                    if (curDistance < distance && curDistance != 0)
                    {
                        closest = col.gameObject;
                        distance = curDistance;
                    }
                }
            }
        }
        target = closest;
    }
}