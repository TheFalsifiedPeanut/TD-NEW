using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState
{
    Idling,
    Searching,
    Delivering,
    Stealing,
    Moving
}

public class Enemy : Unit
{
    [SerializeField] GameObject unitTarget;
    [SerializeField] EnemyState enemyState;
    [SerializeField] int stealAmount;
    [SerializeField] float stealSpeed;
    [SerializeField] int supplyStash;
    [SerializeField] SupplyTower targetSupplyTower;
    [SerializeField] SupplyTowerManager supplyTowerManager;
    [SerializeField] EnemyBase targetEnemyBase;
    [SerializeField] float currentTime = 0;

    void PerformState()
    {
        switch (enemyState)
        {
            case EnemyState.Idling:
                break;
            case EnemyState.Searching:
                if (targetingCondition == TargetingCondition.MoveToBase)
                {
                    targetSupplyTower = null;
                    SupplyTower[] targetSupplyTowers = FindObjectsOfType<SupplyTower>();
                    if (targetSupplyTowers != null && targetSupplyTowers.Length > 0)
                    {
                        targetSupplyTower = targetSupplyTowers[Random.Range(0, targetSupplyTowers.Length)];
                        targetWaypoint = targetSupplyTower;
                        MoveToBase();
                    }
                    
                }
                else if(targetingCondition == TargetingCondition.MoveToEnemyBase)
                {
                    targetEnemyBase = null;
                    EnemyBase[] targetEnemyBases = FindObjectsOfType<EnemyBase>();
                    Debug.Log(targetEnemyBases.Length);
                    if (targetEnemyBases != null && targetEnemyBases.Length > 0)
                    {
                        targetEnemyBase = targetEnemyBases[Random.Range(0, targetEnemyBases.Length)];
                        targetWaypoint = targetEnemyBase;
                        MoveToEnemyBase();
                    }
                }
                enemyState = EnemyState.Moving;
                break;
            case EnemyState.Delivering:
                Debug.Log("Delivering " + supplyStash);
                if (targetEnemyBase != null)
                { 
                    targetEnemyBase.AddEnemyBaseSupply(supplyStash);
                }
                supplyStash = 0;
                enemyState = EnemyState.Searching;
                targetingCondition = TargetingCondition.MoveToBase;
                break;
            case EnemyState.Stealing:
                currentTime += Time.deltaTime;
                if (currentTime >= stealSpeed)
                {
                    supplyStash = supplyTowerManager.RemoveSupply(stealAmount);
                    currentTime = 0;
                    enemyState = EnemyState.Searching;
                    targetingCondition = TargetingCondition.MoveToEnemyBase;
                    //currentWaypoint.TrackSearch(this, currentWaypoint, new PathData());
                }
                break;
            case EnemyState.Moving:
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
       
        currentWaypoint = collision.GetComponent<Waypoint>();
        if (currentWaypoint)
        {
            switch (targetingCondition)
            {
                case TargetingCondition.Waypoint:
                    break;
                case TargetingCondition.MoveToBase:
                    
                    MoveToBase();

                    break;
                case TargetingCondition.MoveToEnemyBase:

                    MoveToEnemyBase();

                    break;
                case TargetingCondition.MoveToResource:
                    break;
                case TargetingCondition.Idle:
                    break;
                
                default:
                    break;
            }
        }
    }
    void MoveToBase()
    {
        if (currentWaypoint == targetWaypoint)
        {
            //SupplyTower supplyTower = (SupplyTower)waypoint;
            //You made it. TO DO: Handle target reach base
            enemyState = EnemyState.Stealing;
            targetingCondition = TargetingCondition.Idle;

        }
        else
        {
            //Keep on going.
            List<TargetableWaypoint> targetableWaypoints = new List<TargetableWaypoint>(currentWaypoint.GetBaseWaypoints());
            TargetableWaypoint targetableWaypoint = targetableWaypoints.Find((x) => x.targetWaypoint == targetWaypoint);
            if (targetableWaypoint.targetWaypoint != null)
            {
                Debug.Log("Start");
                if (targetableWaypoint.waypoints != null)
                {
                    Debug.Log("End");
                    movementTarget = targetableWaypoint.waypoints[Random.Range(0, targetableWaypoint.waypoints.Length)].gameObject;
                }
            }
        }
    }
    void MoveToEnemyBase()
    {
        if (currentWaypoint == targetWaypoint)
        {
            enemyState = EnemyState.Delivering;
            targetingCondition = TargetingCondition.Idle;
        }
        else
        {
            //Keep on going.
            List<TargetableWaypoint> targetableWaypoints = new List<TargetableWaypoint>(currentWaypoint.GetEnemyBaseWaypoints());
            TargetableWaypoint targetableWaypoint = targetableWaypoints.Find((x) => x.targetWaypoint == targetWaypoint);
            if (targetableWaypoint.targetWaypoint)
            {
                Debug.Log("Thing");

                if (targetableWaypoint.waypoints != null)
                {
                    movementTarget = targetableWaypoint.waypoints[Random.Range(0, targetableWaypoint.waypoints.Length)].gameObject;
                }
            }
        }
    }
    private void Start()
    {
        targetingCondition = TargetingCondition.MoveToBase;
    }
    protected override void Update()
    {
        PerformState();
        if (enemyState == EnemyState.Moving)
        {
            base.Update();
        }
    }
}
