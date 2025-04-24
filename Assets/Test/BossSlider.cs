using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossSlider : MonoBehaviour
{
    [SerializeField] private Slider bossslider;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Drog bossScript = GameObject.Find("Boss").GetComponent<Drog>();
        // bossScript.CurHp
        if (bossScript.BossPhase < 2)
        {
            bossslider.value = bossScript.CurHp / bossScript.monsterInfo.health;
        }
    }
}
