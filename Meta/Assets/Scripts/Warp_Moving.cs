using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Warp_Moving : MonoBehaviour
{
    [Header("플레이어 감지 거리")]
    public float interactDistance = 2f;  // F키를 누를 수 있는 거리

    [Header("전환할 미니게임 씬 이름")]
    public string miniGameSceneName = "";
    private Transform player;

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;
    }

    void Update()
    {
        if (player == null) return;

        // 플레이어와 거리 계산
        float distance = Vector2.Distance(player.position, transform.position);

        // 플레이어가 가까이 있고 F키를 누르면 씬 전환
        if (distance <= interactDistance && Input.GetKeyDown(KeyCode.F))
        {
            LoadMiniGame();
        }
    }

    void LoadMiniGame()
    {
        SceneManager.LoadScene(miniGameSceneName);
    }
}
