using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepellingWave : MonoBehaviour
{
    
    public float damage;

    private bool isSizeDown = false;
    private BoxCollider boxCollider;
    private Vector3 targetPos;

    

    private void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
        targetPos = transform.position + new Vector3(transform.position.x, transform.position.y, transform.position.z + 20f);
        Invoke("SizeDown", 0.6f);
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPos, 13f * Time.deltaTime);

        if (isSizeDown)
            if (boxCollider.size.x > 0.1)
            boxCollider.size = new Vector3(boxCollider.size.x - Time.deltaTime * 5, boxCollider.size.y, boxCollider.size.z);

        if (transform.position == targetPos)
        {
            Destroy(gameObject);
        }
    }

    void SizeDown()
    {
        isSizeDown = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            Debug.Log("몬스터 맞음");
        }
        // 데미지 주기 + 슬로우
    }
}
