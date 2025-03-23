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
                    Debug.Log(60 + (damage * 0.2) + " «««ÿ ¿‘»˚");
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
                Debug.Log(100 + (damage * 0.2) + " «««ÿ ¿‘»˚");
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