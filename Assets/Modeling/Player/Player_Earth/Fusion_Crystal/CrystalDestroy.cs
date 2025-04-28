using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalDestroy : MonoBehaviour
{
    void Start()
    {
        Invoke("SelfDestroy", 1f);
    }

    void SelfDestroy()
    {
        Destroy(gameObject);
    }
}
