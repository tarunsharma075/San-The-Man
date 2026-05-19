using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayManager : MonoBehaviour
{
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private GameObject[] prefabs;
    void Start()
    {
        spawnEnemies();
    }

 private void spawnEnemies()
 {
        Vector3 spawnOffset = new Vector3(0, 1f, 0);

        for (int i = 0; i < spawnPoints.Length; i++) { 
  int randomEnemey = Random.Range(0, prefabs.Length);
   GameObject currentSpawnEnemy=  Instantiate(prefabs[randomEnemey], 
   spawnPoints[i].position,
   Quaternion.identity);


            currentSpawnEnemy.transform.position += spawnOffset;
      }
}
}
