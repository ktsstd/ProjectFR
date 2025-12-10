using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtkSkillScroll : PlayerItem
{
    GameObject[] players;
    public GameObject attackSkillOBJ;
    public override void Start()
    {
        base.Start();
    }

    public override void ItemEffect(int _player, Vector3 _usePos)
    {
        Instantiate(attackSkillOBJ);
    }
}
