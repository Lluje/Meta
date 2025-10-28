using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public GameObject obstacleTypeA;  
    public GameObject obstacleTypeB;

    [Header("���� ���� ����")]
    public float minSpawnTime = 1.5f; // �ּ� ���� ����
    public float maxSpawnTime = 3.0f; // �ִ� ���� ����

    [Header("���� ��ġ ����")]
    public Transform spawnPoint;      // ��ֹ��� ��Ÿ�� ���� ��ġ
    public float spawnOffsetX = 10f;  // �÷��̾�κ��� ������ �Ÿ�
    public float spawnY = -2.5f;      // Y��ǥ

    [Header("���� ����")]
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

        // ��ֹ� ����
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
        // �ڿ��� ������ ��ȸ�ϸ鼭 ����
        for (int i = activeObstacles.Count - 1; i >= 0; i--)
        {
            GameObject obstacle = activeObstacles[i];
            if (obstacle == null)
            {
                activeObstacles.RemoveAt(i);
                continue;
            }

            // �÷��̾�� ����� �������� ����� ����
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
