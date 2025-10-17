using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class FlameGrenadeTest : MonoBehaviour
{
    public PlayableDirector playableDirector;
    public float damage;
    float damageDelay = 0f;
    float boomDamage = 1f;
    List<GameObject> monsterInRange = new List<GameObject>();

    public GameObject fireHitEF;
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

                    MonsterAI monster = monsters.GetComponent<MonsterAI>();
                    monster.MonsterDmged(60f + (damage * 0.2f));

                    GameObject damageText = PoolManager.Instance.text_Pools.Get();
                    damageText.transform.position = monster.transform.position;
                    damageText.GetComponent<DamageText>().damage = 60f + (damage * 0.2f);

                    Instantiate(fireHitEF, monsters.transform);
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
            if (other.tag == "Enemy")
            {
                other.GetComponent<MonsterAI>().MonsterDmged(100f + (damage * 0.2f));
                Instantiate(fireHitEF, other.transform);
            }
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

    void SelfDestroy()
    {
        Destroy(gameObject);
    }
}