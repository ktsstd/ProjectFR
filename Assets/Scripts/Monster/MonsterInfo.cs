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
    public float attackCooldown;
    public float attackdDelay;
    public float[] skillRange;
    public float[] skillCooldown;
    public float[] skillDelay;

    public string[] priTarget;

    public bool isBoss;
    public float redistance;
}
