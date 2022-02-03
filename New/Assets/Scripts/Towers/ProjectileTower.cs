using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public enum TargetType
{
    First,
    Last,
    Close,
    Strong
}

public class ProjectileTower : RangedTower
{
    [SerializeField] GameObject projectileObject;
    [SerializeField] TargetType targetType;
    [SerializeField] LayerMask enemyLayer;
    ObjectPool<GameObject> projectilePool;
    protected override void Action()
    {

        
        Collider2D closestTarget = null;
        switch (targetType)
        {
            case TargetType.First:
                break;
            case TargetType.Last:
                break;
            case TargetType.Close:
                Collider2D[] collisions = Physics2D.OverlapCircleAll(new Vector2(transform.position.x, transform.position.y), range, enemyLayer.value);
                float minMagnitudeDistance = Mathf.Infinity;
                for (int i = 0; i < collisions.Length; i++)
                {
                    Vector3 offset = collisions[i].transform.position - transform.position;
                    if (offset.sqrMagnitude < minMagnitudeDistance)
                    {
                        minMagnitudeDistance = offset.sqrMagnitude;
                        closestTarget = collisions[i];
                    }
                }
                break;
            case TargetType.Strong:
                break;
        }
        if (closestTarget != null)
        {
            GameObject projectile = projectilePool.Get();
            projectile.GetComponent<Projectile>().SetTarget(closestTarget.gameObject);
        }


    }

    protected override void Start()
    {
        base.Start();
        projectilePool = new ObjectPool<GameObject>(SpawnProjectile, GetProjectile, ReleaseProjectile, DestroyProjectile, false, 5, 50);
    }

    private GameObject SpawnProjectile()
    {
        GameObject projectile = Instantiate(projectileObject);
        projectile.GetComponent<Projectile>().SubscribeToReleaseProjectile(Release);
        projectile.SetActive(false);
        return projectile;
    }
    private void Release(GameObject projectile)
    {
        projectilePool.Release(projectile);
    }
    private void GetProjectile(GameObject projectile)
    {
        projectile.SetActive(true);
        projectile.transform.position = transform.position;
        projectile.transform.rotation = projectile.GetComponent<Projectile>().GetRotation();
    }
    private void ReleaseProjectile(GameObject projectile)
    {
        projectile.SetActive(false);
    }
    private void DestroyProjectile(GameObject projectile)
    {
        Destroy(projectile);
    }
}
