using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] MonsterAI monsterAIS;
    private float damage;

    void Awake()
    {
        damage = monsterAIS.damage;
    }
}
