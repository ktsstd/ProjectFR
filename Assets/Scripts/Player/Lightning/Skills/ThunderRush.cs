using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderRush : PlayerSkill
{
    public float damage;

    void Start()
    {
        Invoke("SelfDestroy", 1f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            HitOther(other.gameObject, 60f + (damage * 0.5f));
        }
    }
}
