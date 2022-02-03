using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour
{
    [SerializeField] int resources;
    
    public int GetResources()
    {
        return resources;
    }

    public int HarvestingResources(int harvestAmount)
    {
        int currentResources = resources;
        resources -= harvestAmount;
        if (currentResources - harvestAmount < 0)
        {
            return harvestAmount - currentResources;
        }
        return harvestAmount;
    }
}
