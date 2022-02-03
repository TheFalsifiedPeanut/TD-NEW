using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Unit
{
    [SerializeField] GameObject unitTarget;

    private void Start()
    {
        targetingCondition = TargetingCondition.MoveToBase;
    }
}
