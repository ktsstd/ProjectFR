using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class husoofather : MonsterAI
{
    float Dmg = 0;
    float SecTime = 0;
    bool damaged = false;

    public TextMeshProUGUI Damage;
    public TextMeshProUGUI Dps;
    public TextMeshProUGUI Timer;
    public override void Start()
    {
        Damage.text = "Damage: " + Dmg;
        Dps.text = "Dps: " + Dmg / (SecTime);
        Timer.text = "Time: " + (SecTime) + "√ ";
    }
    public override void Update()
    {
        if (damaged)
        {
            SecTime += Time.deltaTime;
        }
        //else
        //{
        //    StartCoroutine(resettimer());
        //}
    }
    //private IEnumerator resettimer()
    //{
    //    yield return new WaitForSeconds(5f);
    //    if (!damaged)
    //    {
    //        SecTime = 0;
    //    }
    //    else
    //    {
    //        damaged = false;
    //    }

    //}
    public override void OnMonsterSpeedDown(float _time, float _moveSpeed)
    {
        
    }
    public override void MonsterDmged(float damage)
    {
        damaged = true;
        //StopCoroutine(resettimer());
        //StartCoroutine(resettimer());
        Dmg += damage;
        Damage.text = "Damage: " + Dmg;
        Dps.text = "Dps: " + Dmg / (Mathf.FloorToInt(SecTime));
        Timer.text = "Time: " + (Mathf.FloorToInt(SecTime)) + "√ ";
    }
    public void OnReset()
    {
        Dmg = 0;
        SecTime = 0;
        Damage.text = "Damage: " + Dmg;
        Dps.text = "Dps: " + Dmg / (Mathf.FloorToInt(SecTime));
        Timer.text = "Time: " + (Mathf.FloorToInt(SecTime)) + "√ ";
    }
}
