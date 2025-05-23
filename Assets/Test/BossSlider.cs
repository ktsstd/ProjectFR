using Photon.Pun;
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
            if (PhotonNetwork.PlayerList.Length > 1)
            {
                if (bossScript.BossPhase < 2)
                {
                    bossslider.value = bossScript.CurHp / bossScript.monsterinfo.health;
                }
                else
                {
                    bossslider.value = bossScript.CurHp / bossScript.BossPhase2Hp;
                }
            }
            else
            {
                if (bossScript.BossPhase < 2)
                {
                    bossslider.value = bossScript.CurHp / (bossScript.monsterinfo.health / 2);
                }
                else
                {
                    bossslider.value = bossScript.CurHp / bossScript.BossPhase2Hp;
                }
            }
            
        }
        else
        {
            bossScript = GameObject.Find("Boss(Clone)").GetComponent<Drog>();
        }
    }
}
