using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStat", menuName = "Player")]

public class PlayerInfo : ScriptableObject
{
    public float hp;
    public float atk;
    public float speed;
    public float dashCoolTime;
    public float[] skillsCoolTime;
}
