using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetLocator : MonoBehaviour
{
    [SerializeField] Transform weapon;
    [SerializeField] ParticleSystem projectileParticles;
    [SerializeField] float range = 15f;       // Range for towers shooting
    Transform target;

    //int boostCost = 50;
    //Bank bank;

    // Start is called before the first frame update
    //void Start()
    //{
    //    target = FindObjectOfType<Enemy>().transform;       //To find the enemy object(i.e. target) using the script component attached to it
    //}                                                       //   but it is ineffective as when the target is diabled/destroyed it can't find a new target

    //private void Start()     // For boostCost
    //{
    //    bank = FindObjectOfType<Bank>();
    //}

    // Update is called once per frame
    void Update()
    {
        FindClosestTarget();
        AimWeapon();
    }

    private void FindClosestTarget()
    {
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        Transform closestTarget = null;
        float maxDistance = Mathf.Infinity;

        foreach (Enemy enemy in enemies)                   //Only good if no. of enemies are less otherwise we will have to apply certain conditions like if enemy is out of range etc. 
        {
            float targetDistance = Vector3.Distance(transform.position, enemy.transform.position);           //To calculate distance b/w tower and enemy
            if(targetDistance < maxDistance )
            {
                closestTarget = enemy.transform;
                maxDistance = targetDistance;
            }
        }

        target = closestTarget;
    }

    private void AimWeapon()
    {
        if(target == null)                //If no enemy is there then to avoid null reference error
        {
            Attack(false);
        }
        else
        {
            float targetDistance = Vector3.Distance(transform.position, target.position);

            weapon.LookAt(target);      // To rotate the weapon to look at the target continuously

            if (targetDistance < range)          //If target is in range then attack else don't
            {
                Attack(true);
            }
            else
            {
                Attack(false);
            }
        }
        
    }

    void Attack(bool isActive)
    {
        var emissionModule = projectileParticles.emission;
        emissionModule.enabled = isActive;
        //if(Input.GetKeyDown(KeyCode.E) && bank != null)          // To increase the rate of fire of bolts, but will increase for all the ballistas which requires a lot of gold,
        //{                                                        // resulting in loosing the game and restart
        //    bank.Withdraw(boostCost);
        //    emissionModule.rateOverTime = 3;
        //}
    }
}
