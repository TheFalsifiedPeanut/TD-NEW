using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SupplyTowerManager : MonoBehaviour
{
    int totalSupply;
    [SerializeField] SupplyTower[] supplyTowers; 
    [SerializeField] TMP_Text supplyText;
    public int GetTotalSupply()
    {
        totalSupply = 0;
        for (int i = 0; i < supplyTowers.Length; i++)
        {
            totalSupply += supplyTowers[i].GetTowerSupply();
        }
        return totalSupply;
    }
    private void Update()
    {
        CheckForSupplyUpdate();
    }
    public void CheckForSupplyUpdate()
    {
        bool commenceUpdate = false;
        for (int i = 0; i < supplyTowers.Length; i++)
        {
            if (supplyTowers[i].GetSupplyUpdate())
            {
                commenceUpdate = true;
                supplyTowers[i].SetSupplyUpdate(false);
            }

        }
        if (commenceUpdate)
        {
            supplyText.text = "Supply: " + GetTotalSupply();
        }
    }
    public void RemoveSupply(int removeAmount)
    {
        for (int i = 0; i < supplyTowers.Length; i++)
        {
            if (supplyTowers[i].GetTowerSupply() >= removeAmount)
            {
                supplyTowers[i].MinusSupply(removeAmount);
                return;
            }
            else
            {
                supplyTowers[i].SetTowerSupply                    (0);
                removeAmount -= supplyTowers[i].GetTowerSupply() ;
            }
            if (removeAmount <=  0)
            {
                return;
            }
        }

    }
}



