using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private float damage;

    void Awake()
    {
        damage = 10f;
    }
}
