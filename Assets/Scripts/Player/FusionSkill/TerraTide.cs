using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TerraTide : MonoBehaviour
{

    public PlayableDirector playableDirector;
    MonsterAI[] monsters;


    void Start()
    {
        playableDirector.Play();

        monsters = FindObjectsOfType<MonsterAI>();

        foreach (MonsterAI monster in monsters)
        {
            monster.OnMonsterKnockBack(gameObject.transform);
        }
        foreach (MonsterAI monster in monsters)
        {
            monster.OnMonsterKnockBack(gameObject.transform);
        }
        foreach (MonsterAI monster in monsters)
        {
            monster.OnMonsterKnockBack(gameObject.transform);
        }
        foreach (MonsterAI monster in monsters)
        {
            monster.OnMonsterKnockBack(gameObject.transform);
        }
        foreach (MonsterAI monster in monsters)
        {
            monster.OnMonsterKnockBack(gameObject.transform);
        }

        Invoke("SelfDestroy", 2f);
    }


    void SelfDestroy()
    {
        Destroy(gameObject);
    }
}