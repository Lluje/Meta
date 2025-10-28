using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground_Loop : MonoBehaviour
{
    public Transform camera;
    public float backgroundWidth = 18.5f;    // 배경 1장의 가로 길이
    public int backgroundCount = 3;  // 배경 몇 장을 연결할지

    private Transform[] backgrounds;

    void Start()
    {
        // 자식 오브젝트들을 배경으로 등록
        int count = transform.childCount;
        backgrounds = new Transform[count];
        for (int i = 0; i < count; i++)
        {
            backgrounds[i] = transform.GetChild(i);
        }

        if (backgrounds.Length < 2)
        {
            Debug.LogError("배경은 최소 2개 이상 필요합니다!");
        }
    }

    void Update()
    {
        foreach (var bg in backgrounds)
        {
            // 플레이어보다 왼쪽으로 충분히 멀어진 배경은 맨 뒤로 이동
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
