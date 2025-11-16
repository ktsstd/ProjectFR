using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class FlameGrenadeTest : PlayerSkill
{
    public PlayableDirector playableDirector;
    public float damage;
    float damageDelay = 0f;
    float boomDamage = 1f;
    List<GameObject> monsterInRange = new List<GameObject>();

    void Start()
    {
        playableDirector.Play();
        Invoke("SelfDestroy", 4f);
    }


    void Update()
    {
        if (damageDelay >= 0)
            damageDelay -= Time.deltaTime;
        if (boomDamage >= 0)
            boomDamage -= Time.deltaTime;

        if (boomDamage <= 0)
        {
            if (damageDelay <= 0)
            {
                foreach (GameObject monsters in monsterInRange)
                {
                    if (monsters == null)
                        monsterInRange.Remove(monsters);

                    HitOther(monsters, 60f + (damage * 0.2f));
                    MonsterAI monster = monsters.GetComponent<MonsterAI>();
                    monster.OnMonsterSpeedDown(4f, 3f);
                }
                damageDelay = 0.5f;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (boomDamage >= 0)
        {
            HitOther(other.gameObject, 100f + (damage * 0.2f));
        }
        if (other.tag == "Enemy")
        {
            monsterInRange.Add(other.gameObject); 
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Enemy")
        {
            monsterInRange.Remove(other.gameObject);
        }
    }
}