using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*Workers takes 10 seconds to gather 12 supply 
A tower loses 1 supply every 1 seconds.
Max supply
    a*/




public class Tower : MonoBehaviour
{
    /// <summary>
    /// Cost for building and upgrading.
    /// </summary>
    [SerializeField] int buildingCosts;
    /// <summary>
    /// Fire rate. Deployment rate. etc.
    /// </summary>
    [SerializeField] float rateOfAction;
    /// <summary>
    /// Cost of maintenance.
    /// </summary>
    [SerializeField] int supplyLoss;
    /// <summary>;
    /// Maximum amount of supply a tower can hold.
    /// </summary>
    [SerializeField] int maxSupply;
    /// <summary>
    /// The current supply in specified building.
    /// </summary>
    [SerializeField] int currentSupply;
    /// <summary>
    /// A flag to tell us if an action is possible.
    /// </summary>
    bool canAction;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        canAction = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (canAction)
        {
            Action();
            StartCoroutine(ActionCooldown());
        }
    }

    protected virtual void Action()
    {

    }
    IEnumerator ActionCooldown()
    {
        canAction = false;
        yield return new WaitForSeconds(rateOfAction);
        canAction = true;
    }
}

