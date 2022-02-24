using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    [SerializeField] GameObject resourceTarget;
    [SerializeField] Resource resource;
    [SerializeField] int harvestAmount;
    [SerializeField] float harvestSpeed;
    [SerializeField] int supplyStash;
    SupplyTower supplyTowerTarget;
    float currentTime;
    Waypoint currentWaypoint;

    private void OnEnable()
    {
        workerState = WorkerState.Searching;
    }
    void PerformState()
    {
        
        switch (workerState)
        {

            case WorkerState.Searching:
                resourceTarget = null;
                Resource[] resources = FindObjectsOfType<Resource>();
                float minMagnitudeDistance = Mathf.Infinity;
                for (int i = 0; i < resources.Length; i++)
                {
                    Vector3 offset = resources[i].transform.position - transform.position;
                    if (offset.sqrMagnitude < minMagnitudeDistance)
                    {
                        minMagnitudeDistance = offset.sqrMagnitude;
                        resourceTarget = resources[i].gameObject;
                    }
                }
                workerState = resourceTarget != null ? WorkerState.Moving : WorkerState.Idling;
                targetingCondition = TargetingCondition.MoveToResource;
                
                break;
            case WorkerState.Moving:
                
                break;
            case WorkerState.Harvesting:
                currentTime += Time.deltaTime;
                if (currentTime >= harvestSpeed)
                {
                    supplyStash = resource.HarvestingResources(harvestAmount);
                    currentTime = 0;
                    workerState = WorkerState.Moving;
                    targetingCondition = TargetingCondition.MoveToBase;
                    //currentWaypoint.TrackSearch(this, currentWaypoint, new PathData());
                }
                break;
            case WorkerState.Delivering:
                Debug.Log("Delivering " + supplyStash);
                if (supplyTowerTarget != null)
                {
                    supplyTowerTarget.AddSupply(supplyStash);
                    supplyTowerTarget.SetSupplyUpdate(true);
                }
                supplyStash = 0;
                workerState = WorkerState.Searching;
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
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (workerState != WorkerState.Harvesting)
        {
            currentTime = 0;
        }
        SupplyTower supplyTowerCollided = other.GetComponent<SupplyTower>();
        if (supplyTowerCollided)
        {
            if (supplyStash > 0)
            {
                workerState = WorkerState.Delivering;
                targetingCondition = TargetingCondition.Idle;
                supplyTowerTarget = supplyTowerCollided;
                return;
            }

        }
        Resource collidedResource = other.GetComponent<Resource>();
        if (collidedResource)
        {
            if (collidedResource.GetResources() > 0)
            {
                currentTime = 0;
                workerState = WorkerState.Harvesting;
                targetingCondition = TargetingCondition.Idle;
                this.resource = collidedResource;
            }
        }
    }
}


