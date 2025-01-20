using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public float range = 20f;
    public float verticalRange;
    public float fireRate;
    private float nextTimeToFire;
    private BoxCollider gunTrigger;
    public EnemyManager enemyManager;
    public float bigDamage = 2f;
    public float smallDamage = 1f;
    public LayerMask raycastLayerMask;
    void Start()
    {
        gunTrigger = GetComponent<BoxCollider>();
        gunTrigger.size = new Vector3(1, verticalRange, range);
        gunTrigger.center = new Vector3(0, 0, range * 0.5f);

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0) && Time.time > nextTimeToFire)
        {
            Fire();
        }
    }

    void Fire()
    {
        //damage enemies
        foreach(var enemy in enemyManager.enemiesInTrigger)
        {
            //tomar la direccion del enemigo
            var dir = enemy.transform.position - transform.position;

            RaycastHit hit;
            if(Physics.Raycast(transform.position, dir, out hit, range * 1.5f, raycastLayerMask))
            {
                if(hit.transform == enemy.transform)
                {
                    float dist = Vector3.Distance(enemy.transform.position, transform.position);
                    if(dist > range * 0.5f)
                    {
                        enemy.TakeDamage(smallDamage);
                    }
                    else
                    {
                        enemy.TakeDamage(bigDamage);
                    }
                }
            }
        }


        nextTimeToFire = Time.time + fireRate;
    }

    private void OnTriggerEnter(Collider other)
    {
        //add potential enemy to shoot
        Enemy enemy = other.transform.GetComponent<Enemy>();

        if(enemy)
        {
            enemyManager.AddEnemy(enemy);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //remove enemy to shoot
        Enemy enemy = other.transform.GetComponent<Enemy>();

        if(enemy)
        {
            enemyManager.RemoveEnemy(enemy);
        }
    }
}
