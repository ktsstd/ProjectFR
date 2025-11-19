using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderRush : PlayerSkill
{
    public float damage;

    public override void Start()
    {
        base.Start();
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
