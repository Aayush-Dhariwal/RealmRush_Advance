using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] int goldReward = 25;
    [SerializeField] int goldPenalty = 25;

    Bank bank;

    // Start is called before the first frame update
    void Start()
    {
        bank = FindObjectOfType<Bank>();
    }

    public void RewardGold()      //When enemy dies, points for tower
    {
        if (bank == null) { return; }
        bank.Deposit(goldReward);
    }

    public void StealGold()         //When enemy crosses the tower, steal from castle 
    {
        if (bank == null) { return; }
        bank.Withdraw(goldPenalty);
    }
}
