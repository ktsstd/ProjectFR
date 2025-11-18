using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FusionLightning : PlayerSkill
{
    void Start()
    {
        Invoke("SelfDestroy", 1f);
    }

    private void OnTriggerEnter(Collider other)
    {
        HitOther(other.gameObject, 100);
    }
}
