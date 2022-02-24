using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupplyTower : Waypoint
{
    int towerSupply = 15;
    bool supplyUpdate;
    public int GetTowerSupply()
    {
        return towerSupply;
    }
    public bool GetSupplyUpdate()
    {
        return supplyUpdate;
    }
    public void SetSupplyUpdate(bool supplyUpdate)
    {
        this.supplyUpdate = supplyUpdate;
    }
    public void SetTowerSupply(int towerSupply)
    {
        this.towerSupply = towerSupply;
        SetSupplyUpdate(true);

    }
    public void AddSupply(int towerSupply)
    {
        this.towerSupply += towerSupply;
        SetSupplyUpdate(true);

    }
    public void MinusSupply(int towerSupply)
    {
        this.towerSupply -= towerSupply;
        SetSupplyUpdate(true);

    }
    void Start()
    {
        towerSupply = 15;
        SetSupplyUpdate(true);
    }
}
