using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    GameObject target;
    [SerializeField] float projectileSpeed;
    [SerializeField] float rotationSpeed;
    [SerializeField] float projectileLife;
    [SerializeField] int damage;
    Coroutine projectileCoroutine;
    Action<GameObject> ReleaseProjectile;

    void Update()
    {
        MoveTowards();
    }
    private void OnEnable()
    {
        projectileCoroutine = StartCoroutine(ProjectileLife());

    }

    public void SetTarget(GameObject target)
    {
        this.target = target;
    }

    private void MoveTowards()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, GetRotation(), Time.deltaTime * rotationSpeed);
        transform.position = Vector3.MoveTowards(transform.position, transform.position + transform.up, Time.deltaTime * projectileSpeed);
    }
    IEnumerator ProjectileLife()
    {
        yield return new WaitForSeconds(projectileLife);
        ReleaseProjectile?.Invoke(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Enemy>())
        {
            FindDamage(other.GetComponent<Enemy>());
            StopCoroutine(projectileCoroutine);
            ReleaseProjectile?.Invoke(gameObject);
        }
    }
    protected virtual void FindDamage(Enemy enemy)
    {
        enemy.RemoveHealth(damage);
    }

    public void SubscribeToReleaseProjectile(Action<GameObject> ReleaseProjectile)
    {
        this.ReleaseProjectile += ReleaseProjectile;
    }

    public Quaternion GetRotation()
    {
        if(target != null)
        {
            Vector3 targetDirection = target.transform.position - this.transform.position;
            float targetAngle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
            return Quaternion.AngleAxis(targetAngle - 90, Vector3.forward);
        }
        else
        {
            StopCoroutine(projectileCoroutine);
            ReleaseProjectile?.Invoke(gameObject);
            return new Quaternion();
        }
    }
}
