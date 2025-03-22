using System.Collections;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject enemySpawnerObject; // Assign EnemySpawner GameObject
    [SerializeField]
    private GameObject door;

    private void Start()
    {
        enemySpawnerObject.SetActive(false);
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            enemySpawnerObject.SetActive(true);
            //StartCoroutine(DisappearDetector());
            door.SetActive(true);
        }
    }

    //IEnumerator DisappearDetector()
    //{
    //    yield return new WaitForSeconds(1f); // Small delay before spawning
    //    EnemySpawn.instance.SpawnEnemies();
    //    yield return new WaitForSeconds(3f); // Wait for enemies to disappear
    //}
}

