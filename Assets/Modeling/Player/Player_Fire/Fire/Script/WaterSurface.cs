using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSurface : MonoBehaviour
{
    public Transform cup; // �� ������Ʈ

    void Update()
    {
        // �� ��ġ�� ���� ��ġ ���󰡱�
        transform.position = cup.position;

        // ȸ���� �����ϰ� �׻� ���� ����
        transform.rotation = Quaternion.Euler(-90, 0, 0);
    }
}