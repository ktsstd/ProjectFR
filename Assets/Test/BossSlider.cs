using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossSlider : MonoBehaviour
{
    [SerializeField] private Slider bossslider;
    Drog bossScript;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (bossScript != null)
        {
            bossScript = GameObject.Find("Boss").GetComponent<Drog>();
        }
        else
        {
            if (bossScript.BossPhase < 2)
            {
                bossslider.value = bossScript.CurHp / bossScript.monsterInfo.health;
            }
        }
    }
}
