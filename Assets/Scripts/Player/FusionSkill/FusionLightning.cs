using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FusionLightning : PlayerSkill
{
    public override void Start()
    {
        base.Start();
        Invoke("SelfDestroy", 1f);
    }

    private void OnTriggerEnter(Collider other)
    {
        HitOther(other.gameObject, 100);
    }
}
