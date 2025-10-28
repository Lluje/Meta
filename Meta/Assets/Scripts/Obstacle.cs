using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public GameObject obstacleTypeA;  
    public GameObject obstacleTypeB;

    [Header("스폰 간격 설정")]
    public float minSpawnTime = 1.5f; // 최소 생성 간격
    public float maxSpawnTime = 3.0f; // 최대 생성 간격

    [Header("생성 위치 설정")]
    public Transform spawnPoint;      // 장애물이 나타날 기준 위치
    public float spawnOffsetX = 10f;  // 플레이어로부터 떨어진 거리
    public float spawnY = -2.5f;      // Y좌표

    [Header("삭제 설정")]
    public float destroyDistance = 10f;

    public Transform player;

    private float timer;
    private float nextSpawnTime;
    private List<GameObject> activeObstacles = new List<GameObject>();

    void Start()
    {
        SetNextSpawnTime();
    }

    void Update()
    {
        timer += Time.deltaTime;

        // 장애물 생성
        if (timer >= nextSpawnTime)
        {
            SpawnObstacle();
            SetNextSpawnTime();
        }

        CleanupObstacles();
    }

    void SpawnObstacle()
    {
        GameObject prefabToSpawn = Random.value < 0.5f ? obstacleTypeA : obstacleTypeB;

        Vector3 spawnPos = new Vector3(player.position.x + spawnOffsetX, spawnY, 0);
        GameObject newObstacle = Instantiate(prefabToSpawn, spawnPos, Quaternion.identity);

        activeObstacles.Add(newObstacle);
    }
    void CleanupObstacles()
    {
        // 뒤에서 앞으로 순회하면서 삭제
        for (int i = activeObstacles.Count - 1; i >= 0; i--)
        {
            GameObject obstacle = activeObstacles[i];
            if (obstacle == null)
            {
                activeObstacles.RemoveAt(i);
                continue;
            }

            // 플레이어보다 충분히 뒤쪽으로 벗어나면 삭제
            if (player.position.x - obstacle.transform.position.x > destroyDistance)
            {
                Destroy(obstacle);
                activeObstacles.RemoveAt(i);
            }
        }
    }
    void SetNextSpawnTime()
    {
        timer = 0f;
        nextSpawnTime = Random.Range(minSpawnTime, maxSpawnTime);
    }
}
