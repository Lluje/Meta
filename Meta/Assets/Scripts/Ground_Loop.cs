using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground_Loop : MonoBehaviour
{
    public Transform camera;
    public float backgroundWidth = 18.5f;    // ��� 1���� ���� ����
    public int backgroundCount = 3;  // ��� �� ���� ��������

    private Transform[] backgrounds;

    void Start()
    {
        // �ڽ� ������Ʈ���� ������� ���
        int count = transform.childCount;
        backgrounds = new Transform[count];
        for (int i = 0; i < count; i++)
        {
            backgrounds[i] = transform.GetChild(i);
        }

        if (backgrounds.Length < 2)
        {
            Debug.LogError("����� �ּ� 2�� �̻� �ʿ��մϴ�!");
        }
    }

    void Update()
    {
        foreach (var bg in backgrounds)
        {
            // �÷��̾�� �������� ����� �־��� ����� �� �ڷ� �̵�
            if (camera.position.x - bg.position.x > backgroundWidth)
            {
                float rightMostX = GetRightmostBackgroundX();
                bg.position = new Vector3(rightMostX + backgroundWidth, bg.position.y, bg.position.z);
            }
        }
    }

    float GetRightmostBackgroundX()
    {
        float maxX = float.MinValue;
        foreach (var bg in backgrounds)
        {
            if (bg.position.x > maxX)
                maxX = bg.position.x;
        }
        return maxX;
    }
}
