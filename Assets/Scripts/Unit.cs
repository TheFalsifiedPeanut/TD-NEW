using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TargetingCondition
{
    Idle,
    Waypoint,
    MoveToBase,
    MoveToResource,
    MoveToEnemyBase,
    MoveToTower,
    MoveToUnit    
}

public class Unit : MonoBehaviour
{
    [SerializeField] protected int health;
    [SerializeField] protected float speed;
    [SerializeField] protected float rotationSpeed;
    [SerializeField] protected GameObject movementTarget;
    [SerializeField] protected TargetingCondition targetingCondition;
    [SerializeField] protected Waypoint targetWaypoint;
    [SerializeField] protected Waypoint currentWaypoint;

    public void SetMovementTarget(GameObject movementTarget)
    {
        this.movementTarget = movementTarget;
    }

    public TargetingCondition GetTargetingCondition()
    {
        return targetingCondition;
    }

    protected virtual void Update()
    {
        if (movementTarget != null)
        {
            Movement();
        }
       
        
    }
    void Movement()
    {
        Vector3 targetDirection = movementTarget.transform.position - this.transform.position;
        float targetAngle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.AngleAxis(targetAngle - 90, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        transform.position = Vector3.MoveTowards(transform.position, transform.position + transform.up, Time.deltaTime * speed);
    }
    public virtual void RemoveHealth(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Death();
        }
    }
    protected virtual void Death()
    {
        Destroy(gameObject);
    }
}
