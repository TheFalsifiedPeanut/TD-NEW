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
    /// <summary>
    /// How often supplyLoss occurs.
    /// </summary>
    [SerializeField] float supplyDrainTimer;
    /// <summary>;
    /// Maximum amount of supply a tower can hold.
    /// </summary>
    [SerializeField] int maxSupply;
    /// <summary>
    /// The current supply in specified building.
    /// </summary>
    [SerializeField] int currentSupply;
    /// <summary>
    /// Undelivered supply designated to the tower by a worker.
    /// </summary>
    [SerializeField] int promisedSupply;
    /// <summary>
    /// A flag that determines when an action occurs.
    /// </summary>
    bool canAction;
    /// <summary>
    /// A flag that determines when supply drain can occur.
    /// </summary>
    bool canDrain;
    
    public  int GetCurrentSupply()
    {
        return currentSupply;
    }public  int GetMaxSupply()
    {
        return maxSupply;
    }
    public int GetPromisedSupply()
    {
        return promisedSupply;
    }
    
    // Start is called before the first frame update
    protected virtual void Start()
    {
        canAction = true;
        canDrain = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (canAction)
        {
            Action();
            StartCoroutine(ActionCooldown());
        }
        if (canDrain)
        {
            Drain();
            StartCoroutine(SupplyDrainCooldown());
        }
    }

    protected virtual void Action()
    {
        
    }
    protected virtual void Drain()
    { 
        currentSupply -= supplyLoss;
        if (currentSupply < 0)
        {
            currentSupply = 0;
        }
        
    }
    IEnumerator ActionCooldown()
    {
        canAction = false;
        yield return new WaitForSeconds(rateOfAction);
        canAction = true;
    }
    IEnumerator SupplyDrainCooldown()
    {

        canDrain = false;
        yield return new WaitForSeconds(supplyDrainTimer);
        canDrain = true;
    }
    public int AddSupply(int supplyToBeAdded)
    {
        promisedSupply -= supplyToBeAdded;
        if (currentSupply + supplyToBeAdded > maxSupply)
        {
            int remainder = currentSupply + supplyToBeAdded - maxSupply;
            currentSupply = maxSupply;
            return remainder;
        }
        else
        {
            currentSupply += supplyToBeAdded;
            return 0;
        }
        
    }
    public void AddPromisedSupply(int supplyToBePromised)
    {
        promisedSupply += supplyToBePromised;
    }
}

