using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Warp_Moving : MonoBehaviour
{
    [Header("�÷��̾� ���� �Ÿ�")]
    public float interactDistance = 2f;  // FŰ�� ���� �� �ִ� �Ÿ�

    [Header("��ȯ�� �̴ϰ��� �� �̸�")]
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

        // �÷��̾�� �Ÿ� ���
        float distance = Vector2.Distance(player.position, transform.position);

        // �÷��̾ ������ �ְ� FŰ�� ������ �� ��ȯ
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
