using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Shockwave : MonoBehaviour
{

    public PlayableDirector playableDirector;
    public float damage;
    public GameObject EarthHitEF;

    void Start()
    {
        playableDirector.Play();
        Invoke("SelfDestroy", 1f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            MonsterAI monsterAI = other.GetComponent<MonsterAI>();
            monsterAI.MonsterDmged(100f + (damage * 0.2f));
            //if (!monsterAI.monsterInfo.isBoss)
            monsterAI.OnMonsterKnockBack(transform);            
            Instantiate(EarthHitEF, other.transform);
        }
    }

    void SelfDestroy()
    {
        Destroy(gameObject);
    }
}
