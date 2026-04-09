using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public GameObject enemy;
    public Transform enemySpawnPos1;
    public Transform enemySpawnPos2; 
    public Transform enemySpawnPos3; 
    public Transform enemySpawnPos4;

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Player") {
            Invoke(nameof(EnemySpawner), 0);
            Destroy(gameObject);
            gameObject.GetComponent<BoxCollider>().enabled = false;
            Debug.Log("Collider Triggered");
        }
    }

    void EnemySpawner() {
        Instantiate(enemy, enemySpawnPos1.position, enemySpawnPos1.rotation);
        Instantiate(enemy, enemySpawnPos2.position, enemySpawnPos2.rotation);
        Instantiate(enemy, enemySpawnPos3.position, enemySpawnPos3.rotation); 
        Instantiate(enemy, enemySpawnPos4.position, enemySpawnPos4.rotation);
        Debug.Log("Enemy Spawned");
    }
}
