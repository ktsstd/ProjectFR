using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSurface : MonoBehaviour
{
    public Transform cup; // 컵 오브젝트

    void Update()
    {
        // 물 위치는 컵의 위치 따라가기
        transform.position = cup.position;

        // 회전은 무시하고 항상 수평 유지
        transform.rotation = Quaternion.Euler(-90, 0, 0);
    }
}