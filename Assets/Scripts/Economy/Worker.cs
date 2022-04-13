using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum WorkerState
{
    Idling,
    Searching,
    Moving,
    Harvesting,
    Delivering
}

public class Worker : Unit
{
    [SerializeField] WorkerState workerState;
    [SerializeField] int harvestAmount;
    [SerializeField] float harvestSpeed;
    [SerializeField] int supplyStash;
    [SerializeField] SupplyTowerManager supplyTowerManager;
    SupplyTower targetSupplyTower;
    Resource targetResource;
    Tower targetTower;
    float currentHarvestTime;
    Action<Worker> onDeath;

    public void SubscribeToOnDeath(Action<Worker> onDeath)
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

        switch (workerState)
        {

            case WorkerState.Searching:
                if (targetingCondition == TargetingCondition.MoveToTower)
                {
                    targetTower = null;
                    Tower[] towers = FindObjectsOfType<Tower>();
                    if (supplyTowerManager.GetTotalSupply() > 0)
                    {
                        foreach (Tower tower in towers)
                        {
                            if (tower.GetCurrentSupply() + tower.GetPromisedSupply() < tower.GetMaxSupply() / 2)
                            {
                                
                                int deliveringSupply = 0;
                                if (tower.GetCurrentSupply() + harvestAmount > tower.GetMaxSupply())
                                {
                                    deliveringSupply = tower.GetMaxSupply() - tower.GetCurrentSupply();
                                }
                                else
                                {
                                    deliveringSupply = harvestAmount;
                                }
                                supplyStash = supplyTowerManager.RemoveSupply(deliveringSupply);
                                tower.AddPromisedSupply(supplyStash);
                                workerState = WorkerState.Moving;
                                targetingCondition = TargetingCondition.MoveToTower;
                                targetTower = tower;
                                targetWaypoint = tower.transform.parent.gameObject.GetComponent<Waypoint>();
                                MoveToTower();
                                //TO DO: Handle no supply in base.
                                return;
                            }
                        }
                    }
                    
                    targetingCondition = TargetingCondition.MoveToResource;
                    return;
                }
                else if (targetingCondition == TargetingCondition.MoveToBase)
                {

                    targetSupplyTower = null;
                    if (currentWaypoint.GetSmartBaseWaypoints() != null && currentWaypoint.GetSmartBaseWaypoints().Length > 0)
                    {
                        targetSupplyTower = (SupplyTower)currentWaypoint.GetSmartBaseWaypoints()[0].targetWaypoint;
                        targetWaypoint = targetSupplyTower;
                        MoveToBase();
                    }
                }
                else if (targetingCondition == TargetingCondition.MoveToResource)
                {
                    targetResource = null;

                    if (currentWaypoint.GetResourceWaypoints() != null && currentWaypoint.GetResourceWaypoints().Length != 0)
                    {
                        for (int i = 0; i < currentWaypoint.GetResourceWaypoints().Length; i++)
                        {
                            if (((Resource)currentWaypoint.GetResourceWaypoints()[i].targetWaypoint).GetResources() > 0)
                            {
                                targetResource = (Resource)currentWaypoint.GetResourceWaypoints()[i].targetWaypoint;
                                targetWaypoint = targetResource;
                                MoveToResource();
                                break;
                            }
                        }
                    }
                }
                workerState = WorkerState.Moving;
                break;
            case WorkerState.Moving:

                break;
            case WorkerState.Harvesting:
                currentHarvestTime += Time.deltaTime;
                if (currentHarvestTime >= harvestSpeed)
                {
                    supplyStash = targetResource.HarvestingResources(harvestAmount);
                    currentHarvestTime = 0;
                    workerState = WorkerState.Searching;
                    targetingCondition = TargetingCondition.MoveToBase;
                    //currentWaypoint.TrackSearch(this, currentWaypoint, new PathData());
                }
                break;
            case WorkerState.Delivering:
                if (targetingCondition == TargetingCondition.MoveToBase)
                {
                    if (targetSupplyTower != null)
                    {
                        targetSupplyTower.AddSupply(supplyStash);
                        targetSupplyTower.SetSupplyUpdate(true);
                        supplyStash = 0;
                        workerState = WorkerState.Searching;
                        targetingCondition = TargetingCondition.MoveToTower;
                    }
                }
                else if (targetingCondition == TargetingCondition.MoveToTower)
                {
                    if (targetTower != null)
                    {
                        supplyStash = targetTower.AddSupply(supplyStash);
                        workerState = WorkerState.Searching;
                        targetingCondition = TargetingCondition.MoveToBase;
                    }
                }

                break;
            case WorkerState.Idling:
                break;
        }
    }
    protected override void Update()
    {
        PerformState();
        if (workerState == WorkerState.Moving)
        {
            base.Update();
        }
    }

    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == movementTarget)
        {
            currentWaypoint = collision.GetComponent<Waypoint>();
            switch (targetingCondition)
            {
                
                case TargetingCondition.Waypoint:
                    break;
                case TargetingCondition.MoveToBase:

                    MoveToBase();

                    break;
                case TargetingCondition.MoveToResource:

                    MoveToResource();

                    break;
                case TargetingCondition.MoveToEnemyBase:
                    break;
                case TargetingCondition.MoveToTower:

                    MoveToTower();

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
        if (currentWaypoint == targetWaypoint )
        {
            workerState = WorkerState.Delivering;
            //targetingCondition = TargetingCondition.Idle;
        } 
        else
        {
            //Keep on going.
            List<TargetableWaypoint> targetableWaypoints = new List<TargetableWaypoint>(currentWaypoint.GetSmartBaseWaypoints());
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
    void MoveToResource()
    {
        if (currentWaypoint == targetWaypoint)
        {
            workerState = WorkerState.Harvesting;
            targetingCondition = TargetingCondition.Idle;
        }
        else
        {
            List<TargetableWaypoint> targetableWaypoints = new List<TargetableWaypoint>(currentWaypoint.GetResourceWaypoints());
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
    void MoveToTower()
    {
        if (currentWaypoint == targetWaypoint)
        {
            workerState = WorkerState.Delivering;
            //targetingCondition = TargetingCondition.Idle;
        }
        else
        {
            List<TargetableWaypoint> targetableWaypoints = new List<TargetableWaypoint>(currentWaypoint.GetTowerWaypoints());
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
}


