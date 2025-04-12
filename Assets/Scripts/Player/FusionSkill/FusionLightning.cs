using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FusionLightning : MonoBehaviour
{
    void Start()
    {
        Invoke("SelfDestroy", 1f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            other.GetComponent<MonsterAI>().MonsterDmged(100);
        }
    }

    void SelfDestroy()
    {
        Destroy(gameObject);
    }
}
