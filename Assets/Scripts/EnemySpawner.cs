using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : BaseEnemyCharacter
{
    public GameObject[] enemyTypes;
    public float spawnTime = 3f;
    public float spawnRadius;

    // Use this for initialization
    void Start()
    {
        base.Init();
        InvokeRepeating("Spawn", spawnTime, spawnTime); //causes this script to run once for each interval spawnTime.
    }

    void Spawn()
    {
        int temp = Random.Range(0, enemyTypes.Length);

        if (Vector3.Distance(this.transform.position, player.transform.position) < spawnRadius)
        {
            Instantiate(enemyTypes[temp], transform.position, transform.rotation);
        }
    }

    override
    protected void Shot(ShotInformation info)
    {
        if (health.takeDamage(info.damage) <= 0)
            Destroy(gameObject);
    }
}