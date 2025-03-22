using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public GameObject enemyPrefab; // Prefab del enemigo
    public int enemyCount = 3; // Número de enemigos a generar
    public float spawnRadius = 5f; // Radio dentro del cual aparecerán los enemigos

    void Start()
    {
        SpawnEnemies();
    }

    public void SpawnEnemies()
    {
        for (int i = 0; i < enemyCount; i++)
        {
            // Generar una posición aleatoria dentro del radio del punto central
            Vector2 randomOffset = Random.insideUnitCircle * spawnRadius;
            Vector3 spawnPosition = new Vector3(
                transform.position.x + randomOffset.x,
                transform.position.y,
                transform.position.z + randomOffset.y
            );

            // Instanciar el enemigo y asignarle el estado inicial de Patrulla
            GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

            // Configurar su estado inicial de patrulla
            EnemyController enemyController = enemy.GetComponent<EnemyController>();
            if (enemyController != null)
            {
                enemyController.SetInitialState(EnemyState.PATROL);
            }
        }
    }
}
