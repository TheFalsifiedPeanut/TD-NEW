using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
struct Wabe
{
    [SerializeField] int enemySpawnAmount;
    [SerializeField] int enemySpawnTime;
    List<Enemy> enemies;

    public int GetEnemySpawnAmount()
    {
        return enemySpawnAmount;
    }
    public int GetEnemySpawnTime()
    {
        return enemySpawnTime;
    }

}


public class EnemyManager : MonoBehaviour
{
    [SerializeField] Transform spawnLocation;
    [SerializeField] List<Wabe> wabes;
    [SerializeField] GameObject enemy;
    List<Enemy> enemies;
    [SerializeField] float waveTimer;
    int currentWaveCount;
    bool canDoWabe;
    // Start is called before the first frame update
    void Start()
    {
        canDoWabe = true;
        enemies = new List<Enemy>();
    }

    // Update is called once per frame
    void Update()
    {
        if(canDoWabe)
        {
            StartCoroutine(WaveCooldown());
            
        }
    }
    IEnumerator WaveCooldown()
    {
        canDoWabe = false;
        Wabe currentWabe = wabes[currentWaveCount];        
        yield return new WaitForSeconds(waveTimer);
        for (int i = 0; i < currentWabe.GetEnemySpawnAmount(); i++)
        {
            Enemy spawnedEnemy = Instantiate(enemy, spawnLocation.position, Quaternion.identity).GetComponent<Enemy>();
            spawnedEnemy.SubscribeToOnDeath(EnemyIsDead);
            
            enemies.Add(spawnedEnemy); 
            yield return new WaitForSeconds(currentWabe.GetEnemySpawnTime());
        }
        currentWaveCount++;
        canDoWabe = true;
    }
    void AreEnemiesDead()
    {
        if (enemies.Count <= 0 && currentWaveCount >= wabes.Count && canDoWabe)
        {
            GameManager.GetGameManager().DisplayWinMessage();
        }
    }
    void EnemyIsDead(Enemy enemy)
    {
        enemies.Remove(enemy);
        AreEnemiesDead();
    }
}
