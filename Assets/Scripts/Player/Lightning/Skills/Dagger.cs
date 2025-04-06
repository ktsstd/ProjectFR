using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Dagger : MonoBehaviour
{
    public Vector3 target;  // ��ǥ ��ġ

    void Start()
    {
        transform.rotation = Quaternion.LookRotation(target - transform.position);
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * 20f);
        if (transform.position == target)
        {
            Destroy(gameObject);
        }
    }
}
