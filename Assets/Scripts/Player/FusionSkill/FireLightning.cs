using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireLightning : PlayerSkill
{
    float damageTime = 1f;

    void Start()
    {
        SoundManager.Instance.PlayPlayerSfx(22, transform.position);
        Invoke("SelfDestroy", 2f);
    }

    private void Update()
    {
        if (damageTime >= 0)
            damageTime -= Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (damageTime >= 0)
        {
            HitOther(other.gameObject, 700f);
        }
    }
}
