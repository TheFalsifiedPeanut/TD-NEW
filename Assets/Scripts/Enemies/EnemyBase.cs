using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : Waypoint
{

    [SerializeField] int EnemyBaseSupply;
    public int GetEnemyBaseSupply()
    {
        return EnemyBaseSupply;
    }
    public void SetEnemyBaseSupply(int EnemyBaseSupply)
    {
        this.EnemyBaseSupply = EnemyBaseSupply;
    }
    public void AddEnemyBaseSupply(int EnemyBaseSupply)
    {
        this.EnemyBaseSupply += EnemyBaseSupply;
    }
    public void MinusEnemyBaseSupply(int EnemyBaseSupply)
    {
        this.EnemyBaseSupply -= EnemyBaseSupply;
    }
}
