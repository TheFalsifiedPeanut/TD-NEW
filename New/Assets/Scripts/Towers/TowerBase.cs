using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBase : MonoBehaviour
{
    Tower tower;
    int TowerPlacedLevel;
    int TowerCurrentLevel;
    [SerializeField] GameObject UI;

    public void IncrementTowerPlacedLevel()
    {
        TowerPlacedLevel++;
        Debug.Log(TowerPlacedLevel);
    }

    public void Start()
    {
        TowerPlacedLevel = 0;
        TowerCurrentLevel = 1;
    }

    private void OnMouseDown()
    {
        if (TowerPlacedLevel < TowerCurrentLevel)
        {
            UI.SetActive(true);
        }
        Debug.Log(TowerPlacedLevel);
    }
    public void SetTower(Tower tower)
    {
        this.tower = tower;
    }
}
