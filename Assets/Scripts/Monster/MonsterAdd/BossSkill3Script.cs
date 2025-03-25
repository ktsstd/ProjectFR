using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(MeshFilter))]
public class BossSkill3Script : MonoBehaviour {
    private float radius = 7f;
    private float angle = 165f;
    private int segments = 50;
    GameObject attackboundaryObj;
    public int damage;
    bool isFadeIn = false;
    Drog bossScript;

    private void Start()
    {
        Mesh mesh = new Mesh();
        Vector3[] vertices = new Vector3[segments + 2];
        int[] triangles = new int[segments * 3];
        MeshCollider meshCollider = gameObject.AddComponent<MeshCollider>();
        meshCollider.sharedMesh = mesh;
        meshCollider.convex = true;
        meshCollider.isTrigger = true;

        // 중심점
        vertices[0] = Vector3.zero;
        // 부채꼴 가장자리의 점들 계산
        for (int i = 0; i <= segments; i++)
        {
            float currentAngle = Mathf.Deg2Rad * (angle * i / segments);
            vertices[i + 1] = new Vector3(Mathf.Cos(currentAngle) * radius, Mathf.Sin(currentAngle) * radius, 0);
        }
        // 삼각형 생성
        for (int i = 0; i < segments; i++)
        {
            triangles[i * 3] = 0;
            triangles[i * 3 + 1] = i + 1;
            triangles[i * 3 + 2] = i + 2;
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        GetComponent<MeshFilter>().mesh = mesh;
        gameObject.SetActive(false);
    }
    public void Starting()
    {
        attackboundaryObj = gameObject;
        bossScript = GetComponentInParent<Drog>();
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        float elapsedTime = 0f;
        float fadedTime = 0.9f; 

        while (elapsedTime <= fadedTime)
        {
            attackboundaryObj.GetComponent<MeshRenderer>().material.color 
                = new Color(1, 0, 0, Mathf.Lerp(0, 0.8f, elapsedTime / fadedTime));

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        isFadeIn = true;
        Invoke("DestroyBoundary", 0.2f);
        yield break;
    }

    private void OnTriggerStay(Collider other)
    {
        if (isFadeIn && other.CompareTag("Player"))
        {
            isFadeIn = false;
            bossScript.Skill3Success(other.gameObject);
            attackboundaryObj.GetComponent<MeshRenderer>().material.color = new Color(1, 0, 0, 0);
            gameObject.SetActive(false);
        }
    }

    void DestroyBoundary()
    {
        if (isFadeIn)
        {
            isFadeIn = false;
            gameObject.SetActive(false);
            bossScript.monsterInfo.attackTimer = bossScript.monsterInfo.attackCooldown;
            bossScript.BossMonsterSkillTimers[2] = bossScript.BossMonsterSkillCooldowns[2];
            bossScript.canMove = true;
            attackboundaryObj.GetComponent<MeshRenderer>().material.color = new Color(1, 0, 0, 0);
        }
        
    }
}
