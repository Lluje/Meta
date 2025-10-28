using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Stack : MonoBehaviour
{
    public GameObject blockPrefab;
    public Transform spawnPoint;
    public float moveRange = 3f;
    public float moveSpeed = 3f;
    public float fallSpeed = 2f;

    private GameObject currentBlock;
    private GameObject lastBlock;
    private int stackCount = 0;

    public Color prevColor;
    public Color nextColor;

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
        }
    }

    void SpawnBlock()
    {
        Vector3 spawnPos = spawnPoint.position + Vector3.up * stackCount;
        currentBlock = Instantiate(blockPrefab, spawnPos, Quaternion.identity);
    }

    void PlaceBlock()
    {
        if (lastBlock == null)
        {
            currentBlock.transform.position = new Vector3(0, currentBlock.transform.position.y, 0);
            lastBlock = currentBlock;
            stackCount++;
            SpawnBlock();
            return;
        }

        float deltaX = currentBlock.transform.position.x - lastBlock.transform.position.x;
        float overlap = Mathf.Abs(deltaX);
        float maxSize = lastBlock.transform.localScale.x;

        if (overlap >= maxSize)
        {
            Destroy(currentBlock);
            Restart();
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

    Color GetRandomColor()
    {
        float r = Random.Range(100f, 250f) / 255f;
        float g = Random.Range(100f, 250f) / 255f;
        float b = Random.Range(100f, 250f) / 255f;

        return new Color(r, g, b);
    }

    void ColorChange(GameObject go)
    {
        Color applyColor = Color.Lerp(prevColor, nextColor, (stackCount % 11) / 10f);

        Renderer rn = go.GetComponent<Renderer>();

        if(rn == null)
        {
            Debug.Log("renderer is null");
            return;
        }

        rn.material.color = applyColor;
        Camera.main.backgroundColor = applyColor - new Color(0.1f, 0.1f, 0.1f);

        if(applyColor.Equals(nextColor) == true)
        {
            prevColor = nextColor;
            nextColor = GetRandomColor();
        }
    }

    void Restart()
    {
        SceneManager.LoadScene("MainScene");
    }
}
