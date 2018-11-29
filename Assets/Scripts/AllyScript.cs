using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AllyScript : BaseAllyCharacter
{
    private NavMeshAgent nav;

    //objects on the map for the NPC to interact with.
    private GameObject player;
    private GameObject target;
    private Animator anim;

    public float range = 50f;
    public float accuracy = .8f;
    // Use this for initialization
    void Start ()
    {
        base.Init();
        nav = GetComponent<NavMeshAgent>(); //get NavMesh component.
        anim = GetComponent<Animator>();
        
        player = GameObject.FindGameObjectWithTag("Player"); //find a player.
        InvokeRepeating("TargetClosestEnemy", 0, 0.25f);
    }
	
	// Update is called once per frame
	void Update ()
    {
        anim.SetFloat("Speed", nav.velocity.magnitude);
        if (Vector3.Distance(transform.position, player.transform.position) >= nav.stoppingDistance)
        {
            anim.SetBool("Aiming", false);
            nav.SetDestination(player.transform.position);
        }
        //movement management
        if (target != null)
        {
            //if a target exists and is within range, shoot at it
            if (Vector3.Distance(transform.position, target.transform.position) < range)
            {
                
                if(gunController.Shoot()) {
                    anim.SetBool("Aiming", true);
                    var targetRotation = Quaternion.LookRotation(target.transform.position - transform.position, Vector3.up);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 2f);
                }
            }
        }
    }

    override
    public void Hit (int damage)
    {
        if (health.takeDamage(damage) <= 0)
        {
            anim.SetTrigger("Dead");
        }
    }

    private void Destroy()
    {
        Destroy(gameObject);
    }

    private void TargetClosestEnemy()
    {
        target = null;
        //get a list of all colliders in radius
        Collider[] objectsInRange = Physics.OverlapSphere(transform.position, range);
        Vector3 position = transform.position; //position of invoking obj.
        GameObject closest = null;
        float distance = Mathf.Infinity;
        foreach (Collider col in objectsInRange)
        {
            if (col.tag == "Enemy")
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
        target = closest;
    }

    private void ShootAnimation()
    {
        anim.SetTrigger("Attack");
    }
}
