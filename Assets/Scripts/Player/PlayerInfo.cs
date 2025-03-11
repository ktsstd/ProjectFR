using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStat", menuName = "Player")]

public class PlayerInfo : ScriptableObject
{
    public float hp;
    public float maxHp;
    public float atk;
    public float speed;
    public float skillA;
    public float skillB;
    public float skillC;
    public float dashCoolTime;
    public float shield;
}
