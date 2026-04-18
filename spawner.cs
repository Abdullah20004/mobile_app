using UnityEngine;
public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject[] obstaclePrefabs;
    public float obstacleSpawnTime = 2f;
    public float initialSpeed = 5f;
    public float speedMultiplier = 0.1f;

    private float timeUntilObstacleSpawn;
    private float currentSpeed;

    private void Start()
    {
        currentSpeed = initialSpeed;
    }

    private void Update()
    {
        currentSpeed += speedMultiplier * Time.deltaTime;
        SpawnLoop();
    }

    private void SpawnLoop()
    {
        timeUntilObstacleSpawn += Time.deltaTime;
        if (timeUntilObstacleSpawn >= obstacleSpawnTime)
        {
            Spawn();
            timeUntilObstacleSpawn = 0;
        }
    }

    private void Spawn()
    {
        GameObject obstacleToSpawn = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)];
        GameObject spawnedObstacle = Instantiate(obstacleToSpawn, transform.position, Quaternion.identity);
        Rigidbody2D rb = spawnedObstacle.GetComponent<Rigidbody2D>();
        rb.linearVelocity = Vector2.left * currentSpeed;
    }
}