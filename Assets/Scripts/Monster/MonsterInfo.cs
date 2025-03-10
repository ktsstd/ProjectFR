using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MonsterInfo", menuName = "Monster")]
public class MonsterInfo : ScriptableObject
{
    public float speed;
    public float damage;
    public float health; 

    public float attackRange;
    public GameObject[] attackboundary;
    public float attackSpeed; 
    public float attackCooldown; 
    public float attackTimer; 

    public string[] priTarget;
}
