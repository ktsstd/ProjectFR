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
    //public float attackSpeed; 
    public float attackCooldown; 
    public float attackTimer; 

    public string[] priTarget;
    public GameObject[] attackboundary;
    //public Vector3 attackPos;

    public bool isBoss;
    public float redistance;
}
