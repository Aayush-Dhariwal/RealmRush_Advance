using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy))]            //To make sure Enemy script is also attached (if we forget) to gameobject when EnemyHealth was attached
public class EnemyHealth : MonoBehaviour
{
    [SerializeField] int maxHitPoints = 5;

    [Tooltip("Add amount to maxHitPoints when enemy dies")]
    [SerializeField] int difficultyRamp = 1;
    int currentHitPoints = 0;

    Enemy enemy;
    
    void OnEnable()                 //Same explanation as for EnemyMover script
    {
        currentHitPoints = maxHitPoints;                   //Initializing with maxHitPoints
    }

    private void Start()
    {
        enemy = GetComponent<Enemy>();    
    }

    void OnParticleCollision(GameObject other)
    {
        //Debug.Log("HIT!!!!");
        ProcessHit();
    }

    void ProcessHit()                             //decreasing the health of enemy when hit and destroying when health = 0
    {
        currentHitPoints--;
        if (currentHitPoints < 1)
        {
            gameObject.SetActive(false);           //When health = 0 gameobject remains in object pool but disabled
            maxHitPoints += difficultyRamp;
            enemy.RewardGold();              // Reward gold to towers when died
        }
    }
}
