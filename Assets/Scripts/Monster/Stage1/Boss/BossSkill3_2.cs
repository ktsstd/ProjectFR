using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSkill3_2 : MonoBehaviour
{
    private void Awake()
    {
        gameObject.GetComponent<ParticleSystem>().Stop();
    }
    private IEnumerator Start()
    {
        yield return new WaitForSeconds(GetComponent<ParticleSystem>().main.duration);
        gameObject.GetComponent<ParticleSystem>().Stop();
    }
}
