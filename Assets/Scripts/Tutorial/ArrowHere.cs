using ExitGames.Client.Photon.StructWrapping;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowHere : MonoBehaviour
{
    private float moveAmount = 0.85f;
    private float moveSpeed = 0.7f;
    private float waitTime = 0f;
    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
        StartCoroutine(MoveUpDown());
    }

    IEnumerator MoveUpDown()
    {
        while (true)
        {
            yield return StartCoroutine(MoveToY(startPos.y + moveAmount));
            yield return new WaitForSeconds(waitTime);
            yield return StartCoroutine(MoveToY(startPos.y));
            yield return new WaitForSeconds(waitTime);
        }
    }

    IEnumerator MoveToY(float targetY)
    {
        while (Mathf.Abs(transform.position.y - targetY) > 0.01f)
        {
            float newY = Mathf.MoveTowards(transform.position.y, targetY, moveSpeed * Time.deltaTime);
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);
            yield return null;
        }
    }
    private void Update()
    {
        transform.Rotate(new Vector3(30f, 0, 0) * Time.deltaTime);
    }
}
