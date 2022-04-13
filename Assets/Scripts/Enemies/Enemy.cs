using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum EnemyState
{
    Idling,
    Searching,
    Delivering,
    Stealing,
    Moving,
    Attacking
}

public class Enemy : Unit
{
    [SerializeField] EnemyState enemyState;
    [SerializeField] int stealAmount;
    [SerializeField] float stealSpeed;
    [SerializeField] int supplyStash;
    [SerializeField] SupplyTowerManager supplyTowerManager;
    SupplyTower targetSupplyTower;
    EnemyBase targetEnemyBase;
    float currentStealTime;
    Action<Enemy> onDeath;
    List<Worker> attackedWorkers;
    DetectWorker detectWorker;
    CircleCollider2D circleCollider2D;

    public void SubscribeToOnDeath(Action<Enemy> onDeath)
    {
        this.onDeath += onDeath;
    }
    protected override void Death()
    {
        base.Death();
        onDeath?.Invoke(this);
    }

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
                        targetSupplyTower = targetSupplyTowers[UnityEngine.Random.Range(0, targetSupplyTowers.Length)];
                        targetWaypoint = targetSupplyTower;
                        MoveToBase();
                    }

                }
                else if (targetingCondition == TargetingCondition.MoveToEnemyBase)
                {
                    targetEnemyBase = null;
                    EnemyBase[] targetEnemyBases = FindObjectsOfType<EnemyBase>();
                    if (targetEnemyBases != null && targetEnemyBases.Length > 0)
                    {
                        targetEnemyBase = targetEnemyBases[UnityEngine.Random.Range(0, targetEnemyBases.Length)];
                        targetWaypoint = targetEnemyBase;
                        MoveToEnemyBase();
                    }
                }
                enemyState = EnemyState.Moving;
                break;
            case EnemyState.Delivering:
                if (targetEnemyBase != null)
                {
                    targetEnemyBase.AddEnemyBaseSupply(supplyStash);
                }
                supplyStash = 0;
                enemyState = EnemyState.Searching;
                targetingCondition = TargetingCondition.MoveToBase;
                break;
            case EnemyState.Stealing:
                currentStealTime += Time.deltaTime;
                if (currentStealTime >= stealSpeed)
                {
                    supplyStash = supplyTowerManager.RemoveSupply(stealAmount);
                    currentStealTime = 0;
                    enemyState = EnemyState.Searching;
                    targetingCondition = TargetingCondition.MoveToEnemyBase;
                    //currentWaypoint.TrackSearch(this, currentWaypoint, new PathData());
                }
                break;
            case EnemyState.Moving:
                break;
            case EnemyState.Attacking:
                Worker worker = movementTarget.GetComponent<Worker>();
                worker.RemoveHealth(1);
                attackedWorkers.Add(worker);
                enemyState = EnemyState.Searching;
                targetingCondition = TargetingCondition.MoveToBase;
                movementTarget = null;
                currentWaypoint = FindObjectOfType<EnemyBase>();
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.gameObject == movementTarget)
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
                        attackedWorkers.Clear();
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
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == movementTarget)
        {
            if (collision.gameObject.GetComponent<Worker>())
            {
                enemyState = EnemyState.Attacking;
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
                if (targetableWaypoint.waypoints != null)
                {
                    movementTarget = targetableWaypoint.waypoints[UnityEngine.Random.Range(0, targetableWaypoint.waypoints.Length)].gameObject;
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

                if (targetableWaypoint.waypoints != null)
                {
                    movementTarget = targetableWaypoint.waypoints[UnityEngine.Random.Range(0, targetableWaypoint.waypoints.Length)].gameObject;
                }
            }
        }
    }
    private void Start()
    {
        detectWorker = GetComponentInChildren<DetectWorker>();
        detectWorker.SubscribeToDetectWorker(WorkerDetected);
        targetingCondition = TargetingCondition.MoveToBase;
        supplyTowerManager = FindObjectOfType<SupplyTowerManager>();
        currentWaypoint = FindObjectOfType<EnemyBase>();
        attackedWorkers = new List<Worker>();
    }
    protected override void Update()
    {
        PerformState();
        if (enemyState == EnemyState.Moving)
        {
            base.Update();
        }
    }
    void WorkerDetected(Worker worker)
    {
        
        if (movementTarget)
        {
            if (supplyStash == 0 && !movementTarget.GetComponent<Worker>() && !attackedWorkers.Contains(worker) && (enemyState == EnemyState.Idling || enemyState == EnemyState.Moving))
            {
                targetingCondition = TargetingCondition.MoveToUnit;
                enemyState = EnemyState.Moving;
                movementTarget = worker.gameObject;

            }
        }
    }
}
