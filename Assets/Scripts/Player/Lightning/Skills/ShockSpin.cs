using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class ShockSpin : PlayerSkill
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
            HitOther(other.gameObject, 100f + (damage * 0.2f));
        }
    }
}
