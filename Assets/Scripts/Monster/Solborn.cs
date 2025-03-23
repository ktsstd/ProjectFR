using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Solborn : MonsterAI
{
    private float reviveHealth;
    public override void Start()
    {
        base.Start();
        reviveHealth = monsterInfo.health;
    }
    public override void MonsterDmged(float damage)
    {
        if (!photonView.IsMine) return;

        monsterInfo.health -= damage;
        Debug.Log("health: " + monsterInfo.health);
        if (monsterInfo.health <= 0) // todo -> revive animation
        {
            canMove = false;
            if (reviveHealth > 0)
            {
                StartCoroutine(Revive());
            }
            else
            {
                PhotonNetwork.Destroy(gameObject);
            }
        }
    }
    IEnumerator Revive()
    {
        for (int i = 0; i < 3; i++)
        {
            monsterInfo.health += reviveHealth / 5;
            yield return new WaitForSeconds(1.3f);
        }
        reviveHealth = 0;
        canMove = true;
    }
}
