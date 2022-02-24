using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBaseManager : MonoBehaviour
{
    int totalSupply;
    [SerializeField] EnemyBase[] enemyBases;

    public int GetTotalSupply()
    {
        totalSupply = 0;
        for (int i = 0; i < enemyBases.Length; i++)
        {
            totalSupply += enemyBases[i].GetEnemyBaseSupply();
        }
        return totalSupply;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public int RemoveSupply(int removeAmount)
    {
        int removedAmount = 0;
        for (int i = 0; i < enemyBases.Length; i++)
        {
            if (enemyBases[i].GetEnemyBaseSupply() >= removeAmount)
            {
                removedAmount += removeAmount;
                enemyBases[i].MinusEnemyBaseSupply(removeAmount);
                return removedAmount;
            }
            else
            {
                removeAmount -= enemyBases[i].GetEnemyBaseSupply();
                removedAmount += enemyBases[i].GetEnemyBaseSupply();
                enemyBases[i].SetEnemyBaseSupply(0);
            }
            if (removeAmount <= 0)
            {
                return removedAmount;
            }
        }
        return removedAmount;
    }
}
