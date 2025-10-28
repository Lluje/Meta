using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Stack : MonoBehaviour
{
    public GameObject blockPrefab;
    public Transform spawnPoint;
    public float moveRange = 3f;
    public float moveSpeed = 3f;
    public float fallSpeed = 2f;
    public float stackHeight = 1.0f;

    public Text scoreTxt;
    int score = 0;
    public GameObject Panel;
    public Text lastscoreTxt;

    private GameObject currentBlock;
    private GameObject lastBlock;
    private int stackCount = 0;

    public FollowCameraY followCamera;

    void Start()
    {
        SpawnBlock();
    }

    void Update()
    {
        if (currentBlock == null) return;

        float moveX = Mathf.Sin(Time.time * moveSpeed) * moveRange;
        currentBlock.transform.position = new Vector3(moveX, currentBlock.transform.position.y, 0);

        // 스페이스로 정지 및 쌓기
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PlaceBlock();
            followCamera.MoveUp();
        }
    }

    void SpawnBlock()
    {
        Vector3 spawnPos = spawnPoint.position + Vector3.up * stackCount;
        currentBlock = Instantiate(blockPrefab, spawnPos, Quaternion.identity);

        SpriteRenderer sr = currentBlock.GetComponent<SpriteRenderer>();
        float t = Mathf.PingPong(stackCount * 0.1f, 1f);
        Color color = Color.HSVToRGB(t, 0.8f, 1f);
        sr.color = color;
    }

    void PlaceBlock()
    {
        if (lastBlock == null)
        {
            currentBlock.transform.position = new Vector3(0, currentBlock.transform.position.y, 0);
            lastBlock = currentBlock;
            stackCount++;
            AddScore(score++);
            SpawnBlock();
            return;
        }

        float deltaX = currentBlock.transform.position.x - lastBlock.transform.position.x;
        float overlap = Mathf.Abs(deltaX);
        float maxSize = lastBlock.transform.localScale.x;

        if (overlap >= maxSize)
        {
            Destroy(currentBlock);
            Panel.SetActive(true);
        }

        float newSize = maxSize - overlap;

        float cutSize = overlap;
        float cutPosX = lastBlock.transform.position.x + (deltaX > 0 ? (newSize / 2f + cutSize / 2f) : -(newSize / 2f + cutSize / 2f));

        currentBlock.transform.localScale = new Vector3(newSize, currentBlock.transform.localScale.y, 1);
        currentBlock.transform.position = new Vector3(
            lastBlock.transform.position.x + deltaX / 2f,
            currentBlock.transform.position.y,
            0
        );

        CreateFallingPiece(cutPosX, cutSize, currentBlock.transform.position.y);

        lastBlock = currentBlock;
        stackCount++;
        AddScore(score++);
        SpawnBlock();
    }

    void CreateFallingPiece(float posX, float size, float posY)
    {
        GameObject fallPiece = Instantiate(blockPrefab, new Vector3(posX, posY, 0), Quaternion.identity);
        fallPiece.transform.localScale = new Vector3(size, currentBlock.transform.localScale.y, 1);

        Rigidbody2D rb = fallPiece.AddComponent<Rigidbody2D>();
        rb.gravityScale = 2f; // 낙하 속도 조절
        Destroy(fallPiece, 2.5f); // 2.5초 후 자동 제거
    }

    public void AddScore(int score)
    {
        scoreTxt.text = score.ToString();
        lastscoreTxt.text = score.ToString();
    }
}
