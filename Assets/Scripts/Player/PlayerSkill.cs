using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkill : MonoBehaviour
{
    public GameObject SkillEffect;
    int playerCode;

    void Start()
    {
        //playerCode = GameManager.Instance.localPlayerCharacter.GetComponent<PlayerController>().elementalCode;
        playerCode = GetComponentInParent<PlayerController>().elementalCode;
    }

    public void SelfDestroy()
    {
        Destroy(gameObject);
    }

    public void HitOther(GameObject _other, float _damage)
    {
        if (_other.tag == "Enemy")
        {
            _other.GetComponent<MonsterAI>().MonsterDmged(_damage, playerCode);
            Instantiate(SkillEffect, _other.transform);

            GameObject damageText = PoolManager.Instance.text_Pools.Get();
            damageText.transform.position = _other.transform.position;
            damageText.GetComponent<DamageText>().damage = _damage;
        }
        // else if (other.tag == 환경오브젝트)
    }

    // 대미지, 대미지 띄우기
}
