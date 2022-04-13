using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : Waypoint
{
    [SerializeField] int resources;

    public int GetResources()
    {
        return resources;
    }
    public int HarvestingResources(int harvestAmount)
    {
        int harvestedValue = 0;
        if (resources - harvestAmount <= 0)
        {
            harvestedValue = resources;
            resources = 0;
            GetComponent<SpriteRenderer>().color = Color.red;
        }
        else
        {
            harvestedValue = harvestAmount;
            resources -= harvestedValue;
        }
        return harvestedValue;
    }
    
}
