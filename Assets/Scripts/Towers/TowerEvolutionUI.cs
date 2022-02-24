using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerEvolutionUI : MonoBehaviour
{
    TowerBase towerBase;
    [SerializeField] GameObject towerObject;
    [SerializeField] int upgradeCost;
    [SerializeField] SupplyTowerManager supplyTowerManager;    
    
    // Start is called before the first frame update
    void Start()
    {
        towerBase = this.transform.parent.parent.GetComponent<TowerBase>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        
        if (supplyTowerManager.GetTotalSupply() >= upgradeCost)
        {
            supplyTowerManager.RemoveSupply( upgradeCost);
            towerBase.IncrementTowerPlacedLevel();
            GameObject tower = Instantiate(towerObject, towerBase.transform);
            towerBase.SetTower(tower.GetComponent<Tower>());
            transform.parent.gameObject.SetActive(false);
        }
    }
    private void OnMouseUp()
    {
        
    }
}
