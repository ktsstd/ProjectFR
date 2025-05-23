using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FusionGrenade : MonoBehaviour
{
    public Vector3 target;  // 목표 위치

    private float duration = 1f; // 이동 시간
    private float height = 2f; // 최고점 높이

    void Start()
    {
        StartCoroutine(MoveParabolic(target, duration));
    }

    private void Update()
    {
        if (transform.position == target)
        {
            Destroy(gameObject);
        }
    }

    IEnumerator MoveParabolic(Vector3 targetPos, float time)
    {
        Vector3 startPos = transform.position;
        float elapsedTime = 0;

        while (elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / time;

            Vector3 currentPos = Vector3.Lerp(startPos, targetPos, t);

            currentPos.y += height * Mathf.Sin(t * Mathf.PI);

            transform.position = currentPos;
            yield return null;
        }

        transform.position = targetPos;
    }
}
