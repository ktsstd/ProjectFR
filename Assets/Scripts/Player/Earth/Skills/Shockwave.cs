using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Shockwave : PlayerSkill
{

    public PlayableDirector playableDirector;
    public float damage;

    public override void Start()
    {
        base.Start();
        playableDirector.Play();
        Invoke("SelfDestroy", 1f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            MonsterAI monsterAI = other.GetComponent<MonsterAI>();
            HitOther(other.gameObject, 100f + (damage * 0.2f));

            //if (!monsterAI.monsterInfo.isBoss)
            monsterAI.OnMonsterKnockBack(transform);
        }
    }
}
